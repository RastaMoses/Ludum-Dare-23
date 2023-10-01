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
        if(other.tag == "Enemy" && other.transform.root.TryGetComponent<HP>(out HP _hp)) {
            _hp.TakeDamage(2);
        }
    }

    private IEnumerator Kill() {
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
