using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Animal : MonoBehaviour
{
    private NavMeshAgent agent;
    public behaviourState currentState;
    public float loiterAngles = 45f;
    private bool isReorinting;
    private GameObject target;

    public enum behaviourState
    {
        loitering, fleeing, pursuing, resting
    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        isReorinting = true;


    }

    void Update()
    {
        Loiter();
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
        }
    }

    protected abstract void LookForTarget();

    
    public virtual void Pursue()
    {

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
