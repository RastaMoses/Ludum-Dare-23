using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_Platform : MonoBehaviour
{
    public float speed = 3;
    public AnimationCurve curve;
    private float timer;
    private Vector3 forward, start;

    private void Start()
    {
        forward = transform.forward;
        start = transform.position;
        transform.eulerAngles = Vector3.zero;
    }

    private void Update()
    {
        if(timer > 99) { return; }
        timer += Time.deltaTime * 1.5f;
        transform.position = Vector3.Lerp(start, start + (forward * speed), curve.Evaluate(timer));
        if(timer > 1) { GetComponent<BoxCollider>().isTrigger = false; timer = 100; }
    }
}
