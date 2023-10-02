using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Rail : MonoBehaviour
{
    public AnimationCurve curve;
    public bool friendly;
    public Color32 nice, evil;
    public LayerMask layerMask, evilMask;
    public BoxCollider col;
    public Vector3 dir;
    public float friendlyTime, damage;
    public VisualEffect vfx;

    private bool damaged = true;
    private float _friendlyTime;
    private Vector3 hitPoint;

    private void Start()
    {
        StartCoroutine(Small());
        _friendlyTime = friendlyTime;

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
                if(collider.gameObject.layer == 11) { friendly = false; SetColor(evil); damage = 5; return; }
            }
            _col = Physics.OverlapSphere(hitPoint, 0.1f);
            foreach (Collider collider in _col)
            {
                if (collider.gameObject.layer == 11) { friendly = false; SetColor(evil); damage = 5; return; }
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
            _friendlyTime -= Time.deltaTime;
            if (_friendlyTime < 0) { friendly = false; SetColor(evil); damage = 5; }
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
        if(other.tag == "Enemy" && friendly && damaged && other.TryGetComponent<HP>(out HP _hp)) {
            _hp.TakeDamage(damage);
            damaged = false;
        }
    }

    public void SetColor(Color32 colour) {
        vfx.SetVector4("color", new Vector4(colour.r, colour.g, colour.b, colour.a));
    }
    public void SetDamaged(bool dmg) { damaged = dmg; }
    public void DecayLine(float amnt) {
        _friendlyTime -= amnt;
    }

    private IEnumerator Small() {
        yield return new WaitForSeconds(0.1f);
        col.size = new Vector3(1.5f, col.size.y, 1.5f);
    }
}
