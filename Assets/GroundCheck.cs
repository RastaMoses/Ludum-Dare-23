using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public FPS_Controller controller;

    private void OnTriggerEnter(Collider other)
    {
        controller.Ground();
    }
}
