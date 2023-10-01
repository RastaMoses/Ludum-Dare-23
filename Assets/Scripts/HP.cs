using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HP : MonoBehaviour
{
    public MeshRenderer[] meshes;
    public Material matFlash;
    public int maxHP;
    public bool isPlayer = false;
    public TextMeshProUGUI text;

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
        if (!isPlayer) { GameObject.FindGameObjectWithTag("Player").transform.root.GetComponent<FPS_Controller>().Kill(); }
    }

    public void TakeDamage(float damage) {
        if (!damagable) { return; }
        damagable = false;
        _currentHP -= damage;
        if (isPlayer) { text.text = "HP: " + _currentHP; }
        if(_currentHP <= 0) { Destroy(gameObject); }
        StartCoroutine(Invincibility());
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

    IEnumerator Invincibility() {
        yield return new WaitForSeconds(1.5f);
        damagable = true;
    }
}
