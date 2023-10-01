using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_Zone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") {
            other.transform.root.GetComponent<HP>().TakeDamage(1);
            other.transform.root.GetComponent<FPS_Controller>().Launch();
        }
    }
}
