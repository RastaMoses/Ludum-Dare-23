using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public FPS_Controller controller;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != controller.gameObject && other.gameObject.layer == 6) { controller.Ground(); }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject != controller.gameObject && other.gameObject.layer == controller.groundLayers) { controller.UnGround(); }
    }
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != controller.gameObject && collision.gameObject.layer == controller.groundLayers) { controller.Ground(); }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject != controller.gameObject && collision.gameObject.layer == controller.groundLayers) { controller.UnGround(); }
    }
    */
}
