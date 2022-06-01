using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Chick : MonoBehaviour
{
    // Chicks were spawned by mating chickens in an earlier version, but was cut because I couldn't be bothered getting it to work well.



    public GameObject grownChicken;
    public float secondsToGrow = 5.0f;
    private float t;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        
        t = secondsToGrow;
        transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
        agent.destination = transform.position + Vector3.forward * 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (t > 0)
        {
            t -= Time.deltaTime;;
            if (t<= 0)
            {
                Grow();
                t = secondsToGrow;
            }
        }
    }

    private void Grow()
    {
        Instantiate(grownChicken, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
