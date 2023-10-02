using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendShape : MonoBehaviour
{
    public float beatLength = 0.4762f, beatspeed = 2, lerpSpeed = 2;
    public SkinnedMeshRenderer mesh;
    public bool onBeat = true;

    private float start = 0, target = 100, lerpTimer;

    private void Start()
    {
        if (!onBeat) { mesh.SetBlendShapeWeight(0, 0); }
        StartCoroutine(MeshChange());
    }

    private void Update()
    {
        lerpTimer += Time.deltaTime * beatLength * lerpSpeed;
        mesh.SetBlendShapeWeight(0, Mathf.Lerp(start, target, lerpTimer));
    }

    IEnumerator MeshChange() {
        yield return new WaitForSeconds(beatLength / beatspeed);
        if (target == 0) { target = 100; start = 0; }
        else { start = 100; target = 0; }
        lerpTimer = -1;
        StartCoroutine(MeshChange());
    }
}
