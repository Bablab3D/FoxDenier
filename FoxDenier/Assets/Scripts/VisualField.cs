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



    // Update is called once per frame
    void Update()
    {

    }

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

        visibleAnimals = visibleAnimals.Where(target => target != null).ToList();

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

    public void FindPursuingTarget() // this is a target that is currently pursuing another target, for moose
    {
        float nearestDistance = transform.localScale.x;
        nearestTarget = null;

        visibleAnimals = visibleAnimals.Where(target => target != null).ToList();


        foreach (GameObject target in visibleAnimals)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < nearestDistance 
                && GetComponentInParent<Animal>().targetType == target.GetComponent<Animal>().agentType 
                && target.GetComponent<Animal>().currentState == Animal.BehaviourState.pursuing)
            {
                nearestDistance = distance;
                nearestTarget = target;
            }
        }
    }

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
                && GetComponentInParent<Animal>().agentType != target.GetComponent<Animal>().agentType
                && target.GetComponent<Animal>().currentState == Animal.BehaviourState.pursuing)
            {
                nearestDistance = distance;
                nearestHuntingPredator = target;
            }
        }
    }


}
