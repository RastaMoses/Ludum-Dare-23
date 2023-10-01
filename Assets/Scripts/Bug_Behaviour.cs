using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug_Behaviour : MonoBehaviour
{
    public float speed = 3;
    public AnimationCurve curve;

    private Transform target;
    private Vector3 startPos;
    private float timer;
    private Rail targetRail;
    private bool chasingPlayer = true;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        if (chasingPlayer)
        {
            transform.LookAt(target);
            transform.position += transform.forward * speed * Time.deltaTime;

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5);
            foreach (Collider col in hitColliders)
            {
                if (col.gameObject.layer == 8 && col.transform.root.TryGetComponent<Rail>(out Rail _rail) && _rail.friendly) {
                    targetRail = _rail;
                    target = col.transform; 
                    chasingPlayer = false; 
                    transform.LookAt(target); 
                    startPos = transform.position;
                    _rail.SetDamaged(false);
                    break; 
                }
            }
            return;
        }

        if(timer < 1)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, target.position, curve.Evaluate(timer));
            return;
        }

        if (targetRail.friendly) { targetRail.DecayLine(Time.deltaTime); }
        else { timer = 0; targetRail = null; target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); chasingPlayer = true; }
    }
}
