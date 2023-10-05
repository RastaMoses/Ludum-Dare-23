using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OneShotVFX : MonoBehaviour
{
    [SerializeField] private float timeToDeath = 3f;
    private void Start()
    {
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {

        yield return new WaitForSeconds(timeToDeath);
        Destroy(gameObject);
    }
}
