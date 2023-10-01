using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artillery_Behaviour : MonoBehaviour
{
    public AnimationCurve curve;
    public float fireRate;
    public float attackDelay;
    public GameObject attackMesh;
    public Material attackMat;

    private GameObject showMesh, attack;
    private Transform target;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        StartCoroutine(Attack());
    }

    private void OnDestroy()
    {
        if(attack != null) {
            Destroy(attack);
            Destroy(showMesh);
        }
    }

    private IEnumerator Attack() {
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
        float timer = 0;
        while(attack.transform.localScale.x != showMesh.transform.localScale.x) {
            timer += Time.deltaTime;
            attack.transform.localScale = Vector3.Lerp(Vector3.zero, showMesh.transform.localScale, curve.Evaluate(timer / attackDelay));
            yield return null;
        }

        Collider[] colliders = Physics.OverlapSphere(attack.transform.position, attack.transform.localScale.x / 2);
        foreach(Collider col in colliders) { 
            if(col.transform.root.TryGetComponent<HP>(out HP _hp)) { _hp.TakeDamage(3); }
            if(col.transform.root.TryGetComponent<Rail>(out Rail _rail)) { _rail.friendly = false; _rail.SetColor(new Color32(255, 0, 0, 255)); }
        }

        Destroy(attack);
        Destroy(showMesh);
    }
}
