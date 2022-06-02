using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INHERITANCE
public class FoxAnimal : Animal
{
    [SerializeField] private float pursueSpeedMultiplier = 2f;

    //public float fleeDistance = 7.0f;
    //private Vector3 fleePoint;

    // POLYMORPHISM
    protected override void GetCaught(AnimalType caughtBy)
    {
        // In a previouis version, there was a Moose that could scare foxes away from the chickens, but I found that the Moose made gameplay more confusing.
        // so now the fox does nothing when it gets caught because there are no animals who pursue and catch the fox.

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

    // POLYMORPHISM
    // foxes run faster when pursuing their target
    public override void Pursue()
    {
        agent.speed = DefaultSpeed * pursueSpeedMultiplier;

        base.Pursue();
    }

    // POLYMORPHISM
    public override void Search()
    {
        visualField.FindNearestTarget();
        base.Search();
    }

    private void LateUpdate()
    {
        // 43.22 is the current angle of the camera. I know this is a bad magic number 
        // but I couldn't be bothered doing the whole make GameManager a singleton and get camera from GameManager thing.
        // this script works for the meantime to get the status indicator UI element to appear correctly on the players screen.

        stateIndicator.transform.eulerAngles = new Vector3(43.22f, 0f, 0f);
        // stateIndicator.transform.rotation = playerCamera.transform.rotation;

        switch (currentState)
        {
            case BehaviourState.loitering:
                stateVoice = "\"i want chicken\"";
                break;
            case BehaviourState.pursuing:
                stateVoice = "\"YUMMY\"";
                break;
            case BehaviourState.resting:
                stateVoice = "*sniff sniff*";
                break;
            case BehaviourState.fleeing:
                stateVoice = "\"AAAAHHHHH\"";
                break;
        }

        stateIndicator.text = stateVoice;
    }

    // When the mooses were around, the foxes would flee away from the chicken and moose.

    //public override void Flee()
    //{
    //    agent.destination = fleePoint;

    //    if (pTimer > 0f)
    //    {
    //        pTimer -= Time.deltaTime;

    //        if (Vector3.Distance(transform.position, fleePoint) < 1.0f)
    //        {
    //            currentState = BehaviourState.resting;
    //            pTimer = pursuitTime;
    //        }
    //        else if (pTimer <= 0f)
    //        {
    //            currentState = BehaviourState.resting;
    //            pTimer = pursuitTime;
    //        }
    //    }
    //}
}
