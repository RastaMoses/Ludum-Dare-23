using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill : MonoBehaviour
{
    public float time = 1;
    void Start()
    {
        StartCoroutine(_Kill());
    }

    private IEnumerator _Kill() {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
