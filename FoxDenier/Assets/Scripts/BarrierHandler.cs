using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierHandler : MonoBehaviour
{
    public float barrierDuration = 5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (barrierDuration > 0)
        {
            // The new barriers that the player create slowly sink into the ground over 5 seconds and then are destroyed

            transform.position = new Vector3(transform.position.x,transform.position.y - (Time.deltaTime / barrierDuration), transform.position.z);
            barrierDuration -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
