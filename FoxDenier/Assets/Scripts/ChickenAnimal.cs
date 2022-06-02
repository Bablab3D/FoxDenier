using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenAnimal : Animal
{
    // there was chicken breeding but I removed it
    // public GameObject babyChicken;

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

    // If the chicken sees a predator, it flees.
    public override void Search()
    {
        visualField.LookForPredator();

        if (visualField.nearestHuntingPredator != null)
        {
            currentState = BehaviourState.fleeing;
            Flee();
        }

        // Cickens don't look for other chickens to mate in the current version
        // But when they did they would only look for chickens to mate if there were no foxes to run from

        //else
        //{
        //    visualField.FindNearestTarget();
        //    base.Search();
        //}
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
                stateVoice = "\"cluck cluck\"";
                break;
            case BehaviourState.pursuing:
                stateVoice = "\"cluck cluck\"";
                break;
            case BehaviourState.resting:
                stateVoice = "*pecks ground*";
                break;
            case BehaviourState.fleeing:
                stateVoice = "\"AAAAHHHHH\"";
                break;
        }
        
        stateIndicator.text = stateVoice;
    }
}
