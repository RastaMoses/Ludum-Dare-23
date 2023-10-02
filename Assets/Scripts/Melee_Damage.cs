using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_Damage : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Kill());
    }

    private void OnTriggerStay(Collider other)
    {

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5);
        foreach (Collider col in hitColliders)
        {
            if (col.tag == "Enemy" && col.transform.root.TryGetComponent<HP>(out HP _hp))
            {
                _hp.TakeDamage(2);
            }
        }
    }

    private IEnumerator Kill() {
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
