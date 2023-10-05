using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_Damage : MonoBehaviour
{
    public int damage;
    void Start()
    {
        StartCoroutine(Kill());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy" && other.transform.root.TryGetComponent<HP>(out HP _hp)) {
            Debug.Log("Enemy Hit");
            _hp.TakeDamage(damage);
        }
    }

    private IEnumerator Kill() {
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
