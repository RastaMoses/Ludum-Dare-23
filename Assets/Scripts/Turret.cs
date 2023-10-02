using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public AnimationCurve curve;
    public LayerMask mask;
    public GameObject bullet;
    public Transform head, shotSpawn;
    public float shotSpeed;

    private Transform target;
    private float timer;
    public Animator anim;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if(target == null) { return; }

        if (timer < shotSpeed * 0.9f)
        {
            head.LookAt(target);
        }

        if (!Physics.Raycast(shotSpawn.position, shotSpawn.forward, Vector3.Distance(shotSpawn.position, target.position), mask)) {
            if (anim.GetCurrentAnimatorStateInfo(1).IsTag("Idle")) { anim.SetTrigger("charge"); }

            timer += Time.deltaTime;

            if (timer >= shotSpeed)
            {
                anim.SetTrigger("Shoot");
                Instantiate(bullet, shotSpawn.position, shotSpawn.rotation);
                GetComponent<SFX>().RandomShot();
                timer = 0;
            }
        }
        else
        {
            anim.SetTrigger("cancelled");
            timer = 0;
        }
    }
}
