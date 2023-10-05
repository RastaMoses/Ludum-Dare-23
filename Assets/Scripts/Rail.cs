using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Rail : MonoBehaviour
{
    public AnimationCurve curve;
    public Color32 friendlyColor, decayColor, corruptedColor;
    public LayerMask layerMask, evilMask;
    public BoxCollider col;
    public float bugCorruptionMultiplier = 2;
    public float friendlyTime, decayTime, damage = 5;
    public VisualEffect vfx;


    //State
    [HideInInspector] public Vector3 dir;
    float stableTime = 10;
    public bool friendly, stable;
    private bool damageEnemy = true;
    private float _decayTime;
    private float _stableTime;
    private float currentBugMultiplier = 1;
    private Vector3 hitPoint;
    [HideInInspector]public bool bugCorrupting;

    private void Start()
    {
        StartCoroutine(Small());
        stable = true;
        friendly = true;
        _stableTime = stableTime;
        _decayTime = decayTime;

        //If shoots straight, get direction of line. Else, run ray-march algorithm
        if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 999, layerMask)) {
            dir = hit.point - transform.position;
            vfx.transform.LookAt(hit.point);
            vfx.transform.localEulerAngles = new Vector3(-90, -90, 0);
            vfx.SetFloat("distance", Vector3.Distance(transform.position, hit.point));
            dir.Normalize();
            hitPoint = hit.point;
            col.transform.position = (hit.point + transform.position) / 2;
            col.transform.localEulerAngles = new Vector3(90, 0, 0);
            col.size = new Vector3(col.size.x, Vector3.Distance(hit.point, transform.position), col.size.z);

            if (!friendly) { return; }

            Collider[] _col = Physics.OverlapSphere(transform.position, 0.1f);
            foreach(Collider collider in _col) { 
                if(collider.gameObject.layer == evilMask) { stable = false; return; }
            }
            _col = Physics.OverlapSphere(hitPoint, 0.1f);
            foreach (Collider collider in _col)
            {
                if (collider.gameObject.layer == evilMask) { stable = false; return; }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (friendly)
        {
            if(stable)
            {
                _stableTime -= Time.deltaTime;

                if (_stableTime < 0)
                {
                    stable = false;
                }
            }
            else
            {
                //Lerp Color to decay color
                _decayTime -= Time.deltaTime * currentBugMultiplier;
                Color newColor = Color.Lerp(decayColor, friendlyColor, _decayTime / decayTime);
                print(newColor);
                vfx.SetVector4("color", newColor);
                if (_decayTime < 0)
                {
                    friendly = false;
                    SetColor(corruptedColor);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && friendly) {
            other.GetComponent<FPS_Controller>().BeginGrind(dir, transform.position, hitPoint);
        }
        if(other.tag == "Player" && !friendly) {
            other.GetComponent<HP>().TakeDamage(damage);
        }
        //On Enemy hit
        if(other.tag == "Enemy" && friendly && damageEnemy && other.TryGetComponent<HP>(out HP _hp)) {
            _hp.TakeDamage(damage);
            damageEnemy = false;
        }
    }

    public void SetColor(Color32 colour) {
        vfx.SetVector4("color", new Vector4(colour.r, colour.g, colour.b, colour.a));
    }
    public void SetDamaged(bool dmg) { damageEnemy = dmg; }
    public void DecayLine() 
    {
        bugCorrupting = true;
        stable = false;
        currentBugMultiplier = bugCorruptionMultiplier;
    }

    private IEnumerator Small() {
        yield return new WaitForSeconds(0.1f);
        col.size = new Vector3(1.5f, col.size.y, 1.5f);
    }
}
