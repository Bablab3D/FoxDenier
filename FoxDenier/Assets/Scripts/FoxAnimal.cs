using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxAnimal : Animal
{
    public float fleeDistance = 7.0f;
    private Vector3 fleePoint;


    protected override void GetCaught(AnimalType caughtBy)
    {
        // In a previouis version, there was a Moose that could scare foxes away from the chickens, but I found that the Moose made gameplay more confusing.

        //if (caughtBy == AnimalType.moose)
        //{
        //    // randomly decides which side of the moose to run to.
        //    int foxSide = ((int)Mathf.Sign(Random.Range(-1f, 1f)));

        //    Vector3 fleeDirection;

        //    if (target != null)
        //    {
        //        fleeDirection = -1 * (this.transform.position - target.transform.position).normalized;
        //    } else
        //    {
        //        fleeDirection = Quaternion.Euler(0, 90 * foxSide, 0) * (this.transform.position - caughtByInstance.transform.position).normalized;
        //    }

        //    fleePoint = fleeDirection * fleeDistance;
        //    caughtByInstance.GetComponentInChildren<VisualField>().nearestTarget = null;
        //    agent.speed *= 3.0f;
            
        //    currentState = BehaviourState.fleeing;
        //}
    }

    public override void Pursue()
    {
        agent.speed = defaultSpeed * 2;

        base.Pursue();
    }

    public override void Search()
    {
        visualField.FindNearestTarget();
        base.Search();
    }
    public override void Flee()
    {
        agent.destination = fleePoint;

        if (pTimer > 0f)
        {
            pTimer -= Time.deltaTime;

            if (Vector3.Distance(transform.position, fleePoint) < 1.0f)
            {
                currentState = BehaviourState.resting;
                pTimer = pursuitTime;
            }
            else if (pTimer <= 0f)
            {
                currentState = BehaviourState.resting;
                pTimer = pursuitTime;
            }
        }
    }
}
