using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenAnimal : Animal
{
    public GameObject babyChicken;


    protected override void GetCaught(AnimalType caughtBy)
    {
        if (caughtBy == AnimalType.fox)
        {
            Destroy(gameObject);
            caughtByInstance.GetComponentInChildren<VisualField>().visibleAnimals.Remove(this.gameObject);
            caughtByInstance.GetComponent<Animal>().target = null;
        }  
        
        // In an earlier version, chickens made little baby chicks when they met up with eachother,
        // but I removed the breeding because I couldn't figure out how best to not let the breeding get out of hand.
        // Also, the breeding was super buggy and sometimes wouldn't work for reasons that I couldn't figure out.

        //if (caughtBy == AnimalType.chicken)
        //{
        //    Instantiate(babyChicken, transform.position + Vector3.back * 3f, babyChicken.transform.rotation);
        //    Debug.Log("made baby chicken");

        //    currentState = BehaviourState.loitering;
        //}
    }

    public override void Search()
    {
        visualField.LookForPredator();

        if (visualField.nearestHuntingPredator != null)
        {
            currentState = BehaviourState.fleeing;
            Flee();
        }

        // Cickens don't look for other chickens to mate in the current version

        //else
        //{
        //    visualField.FindNearestTarget();
        //    base.Search();
        //}
    }
}
