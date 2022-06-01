using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Animal : MonoBehaviour
{
    public TextMeshProUGUI stateIndicator;
    public NavMeshAgent agent;
    public float defaultSpeed = 3.5f;
    public BehaviourState currentState;
    public AnimalType agentType;
    public AnimalType targetType;
    public float loiterAngles = 45f;
    public float pursuitTime = 5.0f;
    public float restTime = 2.0f;
    public float pTimer;
    private float rTimer;
    private bool isReorinting;
    [System.NonSerialized] public GameObject target;
    // public GameObject pursuedByInstance;
    [System.NonSerialized] public GameObject caughtByInstance;
    [System.NonSerialized] public VisualField visualField;

    private Animator animAnim;

    public enum BehaviourState
    {
        loitering, fleeing, pursuing, resting
    }

    public enum AnimalType
    {
        chicken, fox, moose
    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        visualField = GetComponentInChildren<VisualField>();
        animAnim = GetComponentInChildren<Animator>();
        agent.speed = defaultSpeed;
        isReorinting = true;
        stateIndicator = GetComponentInChildren<TextMeshProUGUI>();

        transform.localScale = Vector3.one * Random.Range(0.9f, 1.1f);

        currentState = BehaviourState.loitering;

        pTimer = pursuitTime;
        rTimer = restTime;
    }

    void Update()
    {
        // 43.22 is the current angle of the camera. I know this is a bad magic number 
        // but I couldn't be bothered doing the whole make GameManager a singleton and get camera from GameManager thing.
        // this script works for the meantime to get the status indicator UI element to appear correctly on the players screen.

        stateIndicator.transform.eulerAngles = new Vector3(43.22f, 0f, 0f);
        // stateIndicator.transform.rotation = playerCamera.transform.rotation;

        stateIndicator.text = currentState.ToString();

        // this checks the current state of the animal, which is manipulated by various functions
        switch (currentState)
        {
            case BehaviourState.loitering:
                Loiter();
                break;
            case BehaviourState.pursuing:
                Pursue();
                break;
            case BehaviourState.resting:
                Rest();
                break;
            case BehaviourState.fleeing:
                Flee();
                break;
        }
    }


    public virtual void Loiter()
    {
        Vector3 newDirection;
        float newDistance = 5;
        NavMeshHit hit;

        animAnim.SetBool("Eat_b", false);

        if (isReorinting)
        {
            if (NavMesh.FindClosestEdge(transform.position, out hit, NavMesh.AllAreas))
            {
                // if close to edge, go away from edge, otherwise pick a random new direction
                // will need to rework this if we don't want the animals to run away from obstacles
                if (hit.distance < newDistance)
                {
                    newDirection = (transform.position - hit.position).normalized;
                }
                else
                {
                    newDirection = Quaternion.Euler(0, Random.Range(-loiterAngles, loiterAngles), 0) * transform.forward;
                }

                agent.destination = transform.position + (newDirection * newDistance);
                isReorinting = false;

                
            }
        }

        // if the animal gets close enough to their current random loiter waypoint, reorient and search for prey or predators

        if (agent.remainingDistance < 1f)
        {
            isReorinting = true;
            Search();
        }
    }

    public virtual void Search()
    {
        // this class is overridden in both the fox and the chicken child classes

        if (visualField.nearestTarget != null)
        {
            // the visual field is a rectangular object child of the animal that keeps track of targets and predators

            target = visualField.nearestTarget;
            agent.destination = target.transform.position;

            currentState = BehaviourState.pursuing;
        }
    }
    
    public virtual void Pursue()
    {
        if (target != null)
        {
            agent.destination = target.transform.position;

            if (pTimer > 0f)
            {
                pTimer -= Time.deltaTime;

                // When the animal gets close enough to its target, catch the target

                if (Vector3.Distance(this.transform.position, target.transform.position) < 2.0f)
                {

                    // Debug.Log(this.gameObject + " with agent type " + agentType+ " caught " + target.gameObject);

                    target.GetComponent<Animal>().caughtByInstance = this.gameObject;
                    target.GetComponent<Animal>().GetCaught(agentType);

                    currentState = BehaviourState.resting;
                    pTimer = pursuitTime;
                    
                }
                else if (pTimer <= 0f)
                {
                    currentState = BehaviourState.resting;
                    pTimer = pursuitTime;
                    target = null;
                }
            }
        }
        else
        {
            currentState = BehaviourState.resting;

        }
    }

    public virtual void Rest()
    {
        // play the little eating animation while resting, regardless of whether or not animal just caught something
        animAnim.SetBool("Eat_b", true);

        if (rTimer > 0f)
        {
            agent.destination = transform.position;

            //transform.Rotate(0, Time.deltaTime * 360, 0);

            // I did have the animals rotating while resting but that looked weird so I removed it.


            rTimer -= Time.deltaTime;
            if (rTimer <= 0f)
            {
                agent.speed = defaultSpeed;
                target = null;
                currentState = BehaviourState.loitering;
                rTimer = restTime;
            }
        }
    }

    protected abstract void GetCaught(AnimalType caughtBy);


    public virtual void Flee()
    {
        // Use visual field child object to find nearby predators.

        if (visualField.nearestHuntingPredator != null)
        {
            if (pTimer > 0f)
            {
                pTimer -= Time.deltaTime * 2;
                agent.destination = transform.localPosition - visualField.nearestHuntingPredator.transform.localPosition;

                if (pTimer <= 0f)
                {
                    currentState = BehaviourState.resting;
                    pTimer = pursuitTime;
                }
            }
        }
    }

    // Gizmos just to make sure the navmesh pathfinding is all working
    public void OnDrawGizmos()
    {
        if (agent != null)
        {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(agent.destination, 1f);
        }

    }
}
