using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_Zone : MonoBehaviour
{
    public float damage = 1;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") {
            other.transform.root.GetComponent<HP>().TakeDamage(damage);
            other.transform.root.GetComponent<FPS_Controller>().Launch();
        }
    }
}
