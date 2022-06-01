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
        
        if (caughtBy == AnimalType.chicken)
        {
            Instantiate(babyChicken, transform.position + Vector3.back * 3f, babyChicken.transform.rotation);
            Debug.Log("made baby chicken");

            currentState = BehaviourState.loitering;
        }
    }

    public override void Search()
    {
        visualField.LookForPredator();

        if (visualField.nearestHuntingPredator != null)
        {
            currentState = BehaviourState.fleeing;
            Flee();
        }
        else
        {
            visualField.FindNearestTarget();
            base.Search();
        }
    }
}
