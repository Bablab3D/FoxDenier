using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Animal : MonoBehaviour
{
    public TextMeshProUGUI stateIndicator;
    // public GameObject playerCamera;
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
        agent.speed = defaultSpeed;
        isReorinting = true;

        transform.localScale = Vector3.one * Random.Range(0.9f, 1.1f);

        currentState = BehaviourState.loitering;

        pTimer = pursuitTime;
        rTimer = restTime;
    }

    void Update()
    {
        stateIndicator.transform.eulerAngles = new Vector3(43.22f, 0f, 0f);
        // stateIndicator.transform.rotation = playerCamera.transform.rotation;

        stateIndicator.text = currentState.ToString();

        if (currentState == BehaviourState.loitering)
        {
            Loiter();
        }

        if (currentState == BehaviourState.pursuing)
        {
            Pursue();
        }

        if (currentState == BehaviourState.resting)
        {
            Rest();
        }

        if (currentState == BehaviourState.fleeing)
        {
            Flee();
        }

    }


    public virtual void Loiter()
    {
        Vector3 newDirection;
        float newDistance = 5;
        NavMeshHit hit;

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

        if (agent.remainingDistance < 1f)
        {
            isReorinting = true;
            Search();
        }
    }

    public virtual void Search()
    {
        if (visualField.nearestTarget != null)
        {
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

                if (Vector3.Distance(this.transform.position, target.transform.position) < 2.0f)
                {

                    Debug.Log(this.gameObject + " with agent type " + agentType+ " caught " + target.gameObject);

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
    }

    public virtual void Rest()
    {
        if (rTimer > 0f)
        {
            agent.destination = transform.position;
            transform.Rotate(0, Time.deltaTime * 360, 0);

            rTimer -= Time.deltaTime;
            if (rTimer <= 0f)
            {
                agent.speed = defaultSpeed;
                target = null;
                currentState = BehaviourState.loitering;
                rTimer = pursuitTime;
            }
        }
    }

    protected abstract void GetCaught(AnimalType caughtBy);


    public virtual void Flee()
    {
        if (visualField.nearestHuntingPredator != null)
        {
            if (pTimer > 0f)
            {
                pTimer -= Time.deltaTime * 2;
                agent.destination = transform.localPosition + (transform.position - visualField.nearestHuntingPredator.transform.position);

                if (pTimer <= 0f)
                {
                    currentState = BehaviourState.resting;
                    pTimer = pursuitTime;
                }
            }
        }
    }



    public void OnDrawGizmos()
    {
        if (agent != null)
        {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(agent.destination, 1f);
        }

    }
}
