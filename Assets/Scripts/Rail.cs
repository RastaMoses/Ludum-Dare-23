using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    public bool isStraight;
    public LayerMask layerMask;
    public BoxCollider col;

    private List<Vector3> positions = new List<Vector3>(0);
    public Vector3 dir;
    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        //If shoots straight, get direction of line. Else, run ray-march algorithm
        if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 999, layerMask)) {
            if (isStraight) {
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, hit.point);
                dir = hit.point - transform.position;
                dir.Normalize();
                col.transform.position = (hit.point + transform.position) / 2;
                col.transform.localEulerAngles = new Vector3(90, 0, 0);
                col.size = new Vector3(1, Vector3.Distance(hit.point, transform.position), 1);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player") {
            other.GetComponent<FPS_Controller>().BeginGrind(dir);
        }
    }
}
