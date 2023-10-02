using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OneShotVFX : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {

        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
