using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    public AnimationCurve curve;
    public bool friendly;
    public Color32 nice, evil;
    public LayerMask layerMask;
    public BoxCollider col;
    public Vector3 dir;
    public float friendlyTime, damage;

    private LineRenderer lineRenderer;
    private bool damaged = true;
    private float _friendlyTime;

    private void Start()
    {
        StartCoroutine(Small());
        _friendlyTime = friendlyTime;
        lineRenderer = GetComponent<LineRenderer>();

        //If shoots straight, get direction of line. Else, run ray-march algorithm
        if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 999, layerMask)) {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hit.point);
            dir = hit.point - transform.position;
            dir.Normalize();
            col.transform.position = (hit.point + transform.position) / 2;
            col.transform.localEulerAngles = new Vector3(90, 0, 0);
            col.size = new Vector3(col.size.x, Vector3.Distance(hit.point, transform.position), col.size.z);
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
            lineRenderer.startColor = Color32.Lerp(evil, nice, curve.Evaluate(_friendlyTime / friendlyTime));
            lineRenderer.endColor = Color32.Lerp(evil, nice, curve.Evaluate(_friendlyTime / friendlyTime));
            if (_friendlyTime < 0) { friendly = false; }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && friendly) {
            other.GetComponent<FPS_Controller>().BeginGrind(dir, lineRenderer.GetPosition(0), lineRenderer.GetPosition(1));
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
        lineRenderer.startColor = colour;
        lineRenderer.endColor = colour;
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
