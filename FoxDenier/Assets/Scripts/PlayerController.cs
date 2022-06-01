using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject barrier;
    public Camera GameCamera;
    public float PanSpeed = 10.0f;
    public float ZoomSpeed = 150.0f;
    private GameObject newBarrier;
    private Vector3 spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {

        Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        GameCamera.transform.position = GameCamera.transform.position + new Vector3(move.x, 0, move.y) * PanSpeed * Time.deltaTime;
        float zoom = Input.mouseScrollDelta.y;
        GameCamera.transform.position = GameCamera.transform.position + new Vector3(0, -zoom, 0) * ZoomSpeed * Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            MakeBarrier();
        }
        if (Input.GetMouseButton(0) && newBarrier != null && spawnPoint != null)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 midPoint = (spawnPoint + hit.point) / 2f;
                Vector3 angle = spawnPoint - new Vector3(hit.point.x, 0.5f, hit.point.z);

                newBarrier.transform.position = new Vector3(midPoint.x, 0.5f, midPoint.z);
                newBarrier.transform.localScale = new Vector3(1, 1, Vector3.Distance(hit.point, spawnPoint));
                newBarrier.transform.rotation = Quaternion.LookRotation(angle);

                if (newBarrier.transform.localScale.z > 5f)
                {
                    newBarrier = null;
                    MakeBarrier();
                }

            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            newBarrier = null;
        }
    }

    private void MakeBarrier()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.CompareTag("Ground"))
            {
                spawnPoint = new Vector3(hit.point.x, 0.5f, hit.point.z);
                newBarrier = Instantiate(barrier, spawnPoint, barrier.transform.rotation);
            }
        }
    }
}
