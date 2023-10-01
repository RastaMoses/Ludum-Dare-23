using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    public bool master = false;
    public GameObject sfxObject;
    public AudioClip dashFX, enemyHit, enemyKill, healthPickup, melee, railGrind, shotRecovered, gunEmpty, railGunShot, artillery, playerHit;
    public AudioClip[] randomShot;

    private GameObject persistant;

    private void Start()
    {
        if (master) { return; }
        transform.parent = null;
        StartCoroutine(Kill(GetComponent<AudioSource>().clip.length));
        GetComponent<AudioSource>().Play();
    }

    public void Dash() {
        SFX _sfx = Instantiate(sfxObject, transform.position, transform.rotation).GetComponent<SFX>();
        _sfx.GetComponent<AudioSource>().clip = dashFX;
    }
    public void EnemyHit()
    {
        SFX _sfx = Instantiate(sfxObject, transform.position, transform.rotation).GetComponent<SFX>();
        _sfx.GetComponent<AudioSource>().clip = enemyHit;
    }
    public void EnemyKill()
    {
        SFX _sfx = Instantiate(sfxObject, transform.position, transform.rotation).GetComponent<SFX>();
        _sfx.GetComponent<AudioSource>().clip = enemyKill;
    }
    public void Shoot() {
        SFX _sfx = Instantiate(sfxObject, transform.position, transform.rotation).GetComponent<SFX>();
        _sfx.GetComponent<AudioSource>().clip = railGunShot;
    }
    public void EnemyHealthPickup()
    {
        SFX _sfx = Instantiate(sfxObject, transform.position, transform.rotation).GetComponent<SFX>();
        _sfx.GetComponent<AudioSource>().clip = healthPickup;
    }
    public void Melee()
    {
        SFX _sfx = Instantiate(sfxObject, transform.position, transform.rotation).GetComponent<SFX>();
        _sfx.GetComponent<AudioSource>().clip = melee;
    }
    public void RailGrind(bool onRail)
    {
        if (onRail) { persistant = Instantiate(sfxObject, transform.position, transform.rotation); persistant.GetComponent<AudioSource>().clip = railGrind; }
        else { Destroy(persistant); }
    }
    public void ShotRecovered()
    {
        SFX _sfx = Instantiate(sfxObject, transform.position, transform.rotation).GetComponent<SFX>();
        _sfx.GetComponent<AudioSource>().clip = shotRecovered;
    }
    public void GunEmpty()
    {
        SFX _sfx = Instantiate(sfxObject, transform.position, transform.rotation).GetComponent<SFX>();
        _sfx.GetComponent<AudioSource>().clip = gunEmpty;
    }
    public void RandomShot() {
        SFX _sfx = Instantiate(sfxObject, transform.position, transform.rotation).GetComponent<SFX>();
        int r = Random.Range(0, randomShot.Length);
        _sfx.GetComponent<AudioSource>().clip = randomShot[r];
    }
    public void ArtileryShot() {
        SFX _sfx = Instantiate(sfxObject, transform.position, transform.rotation).GetComponent<SFX>();
        _sfx.GetComponent<AudioSource>().clip = artillery;
    }
    public void PlayerHit() {
        SFX _sfx = Instantiate(sfxObject, transform.position, transform.rotation).GetComponent<SFX>();
        _sfx.GetComponent<AudioSource>().clip = playerHit;
    }

    private IEnumerator Kill(float time) {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
