using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug_Behaviour : MonoBehaviour
{
    public float speed = 3, BPM, minBlend = 0, maxBlend = 100, pause = 8, pulseRate = 2;
    public AnimationCurve yumCurve, pulseCurve;
    public SkinnedMeshRenderer mesh;

    private Transform target;
    private Vector3 startPos;
    private float timer, lerpTimer;
    private Rail targetRail;
    private bool chasingPlayer = true;
    private int multiplier = 1;
    private float beatsPerSecond;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        beatsPerSecond = BPM / 60;
        StartCoroutine(Hold(-1));
    }


    private void Update()
    {
        lerpTimer += Time.deltaTime * multiplier * beatsPerSecond * pulseRate;
        float l = Mathf.Lerp(minBlend, maxBlend, pulseCurve.Evaluate(lerpTimer));
        mesh.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, l);

        if(lerpTimer > 1) { lerpTimer = 1; }
        if(lerpTimer < 0) { lerpTimer = 0; }

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
            transform.position = Vector3.Lerp(startPos, target.position, yumCurve.Evaluate(timer));
            return;
        }

        if (targetRail.friendly) { targetRail.DecayLine(Time.deltaTime); }
        else { timer = 0; targetRail = null; target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); chasingPlayer = true; }
    }

    IEnumerator Hold(int _multiplier) {
        yield return new WaitForSeconds(beatsPerSecond / pulseRate / 2);
        multiplier = 0;
        yield return new WaitForSeconds(beatsPerSecond / pulseRate / 2);
        multiplier = _multiplier;
        StartCoroutine(Hold(multiplier * -1));
    }
}
