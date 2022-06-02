using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class VisualField : MonoBehaviour
{

    public List<GameObject> visibleAnimals;
    public GameObject nearestTarget;
    public GameObject nearestHuntingPredator;
    
    // Start is called before the first frame update
    void Start()
    {
        nearestTarget = null;
    }

    // no update method


    // OnTriggerEnter and OnTriggerExit functions keep a list of animals that the agent (parent object) can currently 'see'

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.GetComponent<Animal>() != null && this.gameObject.transform.parent.gameObject != other.gameObject)
        {
            visibleAnimals.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Animal>() != null)
        {
            visibleAnimals.Remove(other.gameObject);
        }
    }

    public void FindNearestTarget()
    {
        float nearestDistance = transform.localScale.x;
        nearestTarget = null;

        // need to clear list of any empty objects in case they got eaten :)
        visibleAnimals = visibleAnimals.Where(target => target != null).ToList();

        // find the closest animal that is the same type as the agent's target type (eg. a fox's target is a chicken)
        foreach (GameObject target in visibleAnimals)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < nearestDistance && GetComponentInParent<Animal>().targetType == target.GetComponent<Animal>().agentType)
            {
                nearestDistance = distance;
                nearestTarget = target;
            }
        }
    }

    // Sane as FindNearestTarget but looking for predators that are not of the same type as the agent (so that chickens don't think other chickens are predators)
    public void LookForPredator()
    {
        float nearestDistance = transform.localScale.x;
        nearestHuntingPredator = null;

        visibleAnimals = visibleAnimals.Where(target => target != null).ToList();

        foreach (GameObject target in visibleAnimals)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < nearestDistance
                && GetComponentInParent<Animal>().agentType == target.GetComponent<Animal>().targetType
                && GetComponentInParent<Animal>().agentType != target.GetComponent<Animal>().agentType)
            {
                nearestDistance = distance;
                nearestHuntingPredator = target;
                // Debug.Log("fleeing from " + nearestHuntingPredator.gameObject);
            }
        }
    }

    // draw the agent's visual field.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, transform.localScale.x/2);
    }

    // This method was used for the Moose which has now been cut

    //public void FindPursuingTarget() // this is a target that is currently pursuing another target, for moose
    //{
    //    float nearestDistance = transform.localScale.x;
    //    nearestTarget = null;

    //    visibleAnimals = visibleAnimals.Where(target => target != null).ToList();


    //    foreach (GameObject target in visibleAnimals)
    //    {
    //        float distance = Vector3.Distance(transform.position, target.transform.position);
    //        if (distance < nearestDistance 
    //            && GetComponentInParent<Animal>().targetType == target.GetComponent<Animal>().agentType 
    //            && target.GetComponent<Animal>().currentState == Animal.BehaviourState.pursuing)
    //        {
    //            nearestDistance = distance;
    //            nearestTarget = target;
    //        }
    //    }
    //}
}
