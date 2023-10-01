using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{
    public Image shotBar;
    public AnimationCurve curve;
    public LayerMask mask;
    public GameObject bullet;
    public Transform head, shotSpawn;
    public float shotSpeed;

    private Transform target;
    private float timer;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if(target == null) { return; }

        shotBar.fillAmount = curve.Evaluate(timer / shotSpeed);

        if (timer < shotSpeed * 0.9f)
        {
            head.LookAt(target);
        }

        if (!Physics.Raycast(shotSpawn.position, shotSpawn.forward, Vector3.Distance(shotSpawn.position, target.position), mask)) {
            timer += Time.deltaTime;

            if (timer >= shotSpeed)
            {
                Instantiate(bullet, shotSpawn.position, shotSpawn.rotation);
                GetComponent<SFX>().RandomShot();
                timer = 0;
            }
        }
        else
        {
            timer = 0;
        }
    }
}
