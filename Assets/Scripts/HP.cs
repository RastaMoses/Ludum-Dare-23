using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    public MeshRenderer[] meshes;
    public Material matFlash;
    public int maxHP, value = 1;
    public bool isPlayer = false;

    private float _currentHP;
    private bool damagable = true;
    private Material[] defaultMat;

    private void Start()
    {
        defaultMat = new Material[meshes.Length];

        for(int i = 0; i < meshes.Length; i++) {
            defaultMat[i] = meshes[i].material;
        }

        _currentHP = maxHP;
    }

    private void OnDestroy()
    {
        if (!isPlayer) { GameObject.FindGameObjectWithTag("Player").transform.root.GetComponent<FPS_Controller>().Kill(value); }
    }

    public void TakeDamage(float damage) {
        if (!damagable) { return; }
        _currentHP -= damage;

        if (isPlayer) { StartCoroutine(Invincibility()); GetComponent<FPS_Controller>().ui.UpdateHealth(_currentHP); GetComponent<FPS_Controller>().LoseMultiplier(); }
        else { GetComponent<SFX>().EnemyHit(); }
        if(_currentHP <= 0) { Destroy(gameObject); }
        
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
        yield return new WaitForSeconds(1.5f);
        damagable = true;
    }
}
