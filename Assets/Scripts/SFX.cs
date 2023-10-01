using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    public bool master = false;
    public GameObject sfxObject;
    public AudioClip dashFX, enemyHit;


    private void Start()
    {
        if (master) { return; }
        transform.parent = null;
        StartCoroutine(Kill(GetComponent<AudioSource>().clip.length));
        GetComponent<AudioSource>().Play();
    }

    public void Dash() {
        SFX _sfx = Instantiate(sfxObject, transform).GetComponent<SFX>();
        _sfx.GetComponent<AudioSource>().clip = dashFX;
    }
    public void EnemyHit() {
        SFX _sfx = Instantiate(sfxObject, transform).GetComponent<SFX>();
        _sfx.GetComponent<AudioSource>().clip = enemyHit;
    }

    private IEnumerator Kill(float time) {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
