using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class HP : MonoBehaviour
{
    //SerializeParams
    //public SkinnedMeshRenderer[] meshes;
    [Header("All")]
    public int maxHP;
    public bool isPlayer = false;
    [Header("Enemies")]
    public int scoreValue;
    public GameObject hitVFX;
    public GameObject deathVFX;
    public string enemyName;
    [Header("Player")]
    public float criticalHealthThreshold = 25;
    public AudioSource aS;

    //public float hitFlashTime = 0.7f;

    //State
    private float _currentHP;
    private bool damagable = true;
    private Material[] defaultMat;

    //Cached Components
    LevelManager level;
    FPS_Controller fps_controller;
    private void Awake()
    {
        level = FindObjectOfType<LevelManager>();
        fps_controller = FindObjectOfType<FPS_Controller>();
    }

    private void Start()
    {
        transform.parent = null;

        /*
        defaultMat = new Material[meshes.Length];

        for(int i = 0; i < meshes.Length; i++) {
            defaultMat[i] = meshes[i].material;
        }
        */

        _currentHP = maxHP;
    }


    private void Update()
    {
        if(_currentHP > criticalHealthThreshold) { aS.Stop(); }
    }

    public void TakeDamage(float damage) {
        if (!damagable) { return; }
        _currentHP -= damage;
        if(_currentHP <= criticalHealthThreshold && isPlayer) { aS.Play(); }

        if (isPlayer) 
        {
            GetComponent<SFX>().PlayerHit(); 
            StartCoroutine(Invincibility()); 
            fps_controller.ui.UpdateHealth(_currentHP); 
            GetComponent<Score>().IncreaseMultiplier(-1);
            level.BonusNoDamage();

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
                fps_controller.Kill(scoreValue, enemyName); 
                level.EnemyKilled();
                Destroy(gameObject);
            }
            else
            {
                fps_controller.enabled = false;
                GetComponent<AudioSource>().volume = 0f;
            }
        }
    }

    void Flash() {
        var flash = Instantiate(hitVFX, transform.position, Quaternion.identity);
        
    }
    void KillVFX()
    {
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
