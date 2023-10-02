using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    public MeshRenderer[] meshes;
    public GameObject deathVFX;
    public Material matFlash;
    public int maxHP, value = 1;
    public AudioSource aS;
    public bool isPlayer = false;

    private float _currentHP;
    private bool damagable = true;
    private Material[] defaultMat;

    private void Start()
    {
        transform.parent = null;

        defaultMat = new Material[meshes.Length];

        for(int i = 0; i < meshes.Length; i++) {
            defaultMat[i] = meshes[i].material;
        }

        _currentHP = maxHP;
    }


    private void Update()
    {
        if(_currentHP > 40) { aS.Stop(); }
    }

    public void TakeDamage(float damage) {
        if (!damagable) { return; }
        _currentHP -= damage;
        if(_currentHP <= 40 && isPlayer) { aS.Play(); }

        if (isPlayer) { GetComponent<SFX>().PlayerHit(); StartCoroutine(Invincibility()); GetComponent<FPS_Controller>().ui.UpdateHealth(_currentHP); GetComponent<FPS_Controller>().LoseMultiplier(1); }
        else if (_currentHP > 0) { GetComponent<SFX>().EnemyHit(); }
        else { GetComponent<SFX>().EnemyKill(); 
            GameObject fx = Instantiate(deathVFX, transform.position + new Vector3(0, 2, 0), transform.rotation);
            fx.transform.localScale = new Vector3(5, 5, 5);
        }

        if (_currentHP <= 0) {
            if (!isPlayer) { GameObject.FindGameObjectWithTag("Player").transform.root.GetComponent<FPS_Controller>().Kill(value); }
            Destroy(gameObject); }
        
        for (int i = 0; i < meshes.Length; i++)
        {
            meshes[i].material = matFlash;
        }
        StartCoroutine(Flash());
    }

    IEnumerator Flash() {
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < meshes.Length; i++)
        {
            meshes[i].material = defaultMat[i];
        }
    }

    IEnumerator Invincibility()
    {
        damagable = false;
        yield return new WaitForSeconds(0.75f);
        damagable = true;
    }

    public void InvulnerableSpawn()
    {
        StartCoroutine(Invincibility());
    }
}
