using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class HP : MonoBehaviour
{
    public SkinnedMeshRenderer[] meshes;
    public GameObject deathVFX;
    public float hitFlashTime = 0.7f;
    public int maxHP, value = 1;
    public AudioSource aS;
    public bool isPlayer = false;

    private float _currentHP;
    private bool damagable = true;
    private Material[] defaultMat;
    public GameObject hitVFX;

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
        if(_currentHP <= 25 && isPlayer) { aS.Play(); }

        if (isPlayer) 
        {
            GetComponent<SFX>().PlayerHit(); 
            StartCoroutine(Invincibility()); 
            GetComponent<FPS_Controller>().ui.UpdateHealth(_currentHP); 
            GetComponent<Score>().IncreaseMultiplier(-1); 
        }
        else if (_currentHP > 0) 
        {
            GetComponent<SFX>().EnemyHit();
            Flash();
        }
        else { GetComponent<SFX>().EnemyKill(); 
            Flash();
            KillVFX();
        }

        if (_currentHP <= 0) {
            if (!isPlayer) 
            { 
                GameObject.FindGameObjectWithTag("Player").transform.root.GetComponent<FPS_Controller>().Kill(value); 
                FindObjectOfType<LevelManager>().EnemyKilled();
                Destroy(gameObject);
            }
            else
            {
                FindObjectOfType<FPS_Controller>().enabled = false;
                GetComponent<AudioSource>().volume = 0f;
            }
        }
    }

    void Flash() {
        var flash = Instantiate(hitVFX, transform.position, Quaternion.identity);
        
    }
    void KillVFX()
    {
        Debug.Log("Death VFX");
        Instantiate(deathVFX, transform.position, Quaternion.identity);
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
