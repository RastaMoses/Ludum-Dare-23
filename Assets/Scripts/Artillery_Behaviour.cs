using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artillery_Behaviour : MonoBehaviour
{
    public float damage;
    public AnimationCurve curve;
    public float fireRate, delay;
    public float attackDelay;
    public GameObject attackMesh, deathVFX;
    public Material attackMat;

    private GameObject showMesh, attack;
    private Transform target;
    public GameObject bug;
    public Animator anim;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        StartCoroutine(Delay());
    }

    private void OnDestroy()
    {
        if(attack != null) {
            Destroy(attack);
            Destroy(showMesh);
        }
        for (int i = 0; i < 3; i++)
        {
            Vector3 spawn = Random.onUnitSphere;
            spawn.y += 1;
            HP _hp = Instantiate(bug, spawn, transform.rotation).GetComponent<HP>();
            _hp.InvulnerableSpawn();
        }
    }

private IEnumerator Delay() {
        yield return new WaitForSeconds(delay);
        StartCoroutine(Attack());
    }

    private IEnumerator Attack() {
        anim.SetTrigger("spawnball");
        yield return new WaitForSeconds(fireRate);
        showMesh = Instantiate(attackMesh, target.position, transform.rotation);
        attack = Instantiate(attackMesh, target.position, transform.rotation);
        attack.GetComponent<MeshRenderer>().material = attackMat;
        attack.transform.localScale = new Vector3(0, 0, 0);
        StartCoroutine(AttackDelay());
        yield return new WaitForSeconds(attackDelay);
        StartCoroutine(Attack());
    }

    private IEnumerator AttackDelay() {
        anim.SetTrigger("attack");
        float timer = 0;
        GetComponent<SFX>().ArtileryShot();
        while(attack.transform.localScale.x != showMesh.transform.localScale.x) {
            timer += Time.deltaTime;
            attack.transform.localScale = Vector3.Lerp(Vector3.zero, showMesh.transform.localScale, curve.Evaluate(timer / attackDelay));
            yield return null;
        }
        GetComponent<SFX>().RandomShot();
        Collider[] colliders = Physics.OverlapSphere(attack.transform.position, attack.transform.localScale.x / 2);
        foreach(Collider col in colliders) { 
            if(col.transform.root.TryGetComponent<HP>(out HP _hp) && _hp.isPlayer) { _hp.TakeDamage(damage); }
            //if(col.transform.root.TryGetComponent<Rail>(out Rail _rail)) { _rail.friendly = false; _rail.SetColor(new Color32(255, 0, 0, 255)); }
        }

        Destroy(attack);
        Destroy(showMesh);
    }
}
