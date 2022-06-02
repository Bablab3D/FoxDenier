using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Animal : MonoBehaviour
{
    [field: SerializeField] public AnimalType agentType { get; protected set; }
    [field: SerializeField] public AnimalType targetType { get; protected set; }
    public BehaviourState currentState { get; protected set; }

    // this is probably overkill but this setter validation is to demonstrate encapsulation for the Unity Learn OOP deliverable
    [SerializeField] [Tooltip("please don't set this to negative")] private float m_DefaultSpeed = 3.5f;
    public float DefaultSpeed
    {
        get { return m_DefaultSpeed; }
        set
        {
            if (value < 0.0f)
            {
                Debug.Log("default speed can not be negative");
            }
            else
            {
                m_DefaultSpeed = value;
            }
        }
    }

    // public variables that affect animal behvaiour
    public float loiterAngles = 45f;
    public float pursuitTime = 5.0f;
    public float restTime = 2.0f;

    // private or protected variables that probably could be declared in their methods but
    // I couldn't figure out how to do that without causing issues.
    protected float pTimer;
    private float rTimer;
    private bool isReorinting;
    protected string stateVoice;

    // objects and classes that let this class interact with other animals and the visual field child object helper
    // target is public because the target needs to be cleared by the agent's victim if it's been eaten
    public GameObject target;
    protected GameObject caughtByInstance;
    protected VisualField visualField;
    protected TextMeshProUGUI stateIndicator;
    private Animator animAnim;
    protected NavMeshAgent agent;
    // public GameObject pursuedByInstance;

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
        agent.speed = DefaultSpeed;
        isReorinting = true;
        stateIndicator = GetComponentInChildren<TextMeshProUGUI>();

        transform.localScale = Vector3.one * Random.Range(0.9f, 1.1f);

        currentState = BehaviourState.loitering;

        pTimer = pursuitTime;
        rTimer = restTime;
    }

    void Update()
    {
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
                agent.speed = DefaultSpeed;
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
