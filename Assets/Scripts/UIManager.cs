using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Threading;

public class UIManager : MonoBehaviour
{
    public SFX sfx;

    //Serialize Params
    [Header("UI Elements")]
    [Header("Reticle")]
    [SerializeField] [Range(0,4)]float debugGun;
    [SerializeField] [Range(0,1)]float debugPlatform;
    [SerializeField] Image crosshair;
    [SerializeField] List<Image> dashes;
    [SerializeField] Image platformChargeSprite;
    [SerializeField] public Image platformSlider;
    [SerializeField] List<Image> gunCharges;
    [SerializeField] public Image gunSlider;
    [Header("Score")]
    [SerializeField] GameObject newPointsPrefab;
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] TextMeshProUGUI scoreBG;
    [SerializeField] TextMeshProUGUI scoreMultiplier;
    [SerializeField] VerticalLayoutGroup verticalLayerGroup;
    [SerializeField] float pointsTime;
    [Header("Objectives")]
    [SerializeField] TextMeshProUGUI objectiveMain;
    [SerializeField] TextMeshProUGUI objectiveBonus;
    [SerializeField] TextMeshProUGUI bonusPoints;
    [Header("Health")]
    [SerializeField] float healthPicChangeThreshhold = 40;
    [SerializeField] Image healthBG;
    [SerializeField] List<Sprite> healthBGs;
    [SerializeField] Image healthPic;
    [SerializeField] List<Sprite> healthPics;
    [SerializeField] float healthAnimTime = 2f;
    [SerializeField] public Image healthSlider;
    [SerializeField] public Image healthSliderRed;
    [SerializeField] AnimationCurve redHealthAnimCurve;
    [SerializeField] float redHealthAnimSpeed;
    [SerializeField] Color lowHealthBar;
    [SerializeField] TextMeshProUGUI lifes;

    [Header("Debug")]
    [SerializeField] float debugHealth = 100f;
    //State
    List<GameObject> pointsWaitingList = new List<GameObject>();
    float health = 100;
    float redHealth = 100;
    Coroutine healthPicCoroutine;
    bool healthPicCorIsRunning;
    Coroutine redHealthCoroutine;
    bool redHealthCorIsRunning;


    #region Public Update Visuals Functions
    public void UpdateHealth(float currentHealth)
    {
        //Slider
        float relativeHP = currentHealth / 100;
        healthSlider.fillAmount = relativeHP;

        if (redHealthCorIsRunning)
        {
            StopCoroutine(redHealthCoroutine);
        }
        redHealthCoroutine = StartCoroutine(RedHealthAnimation (currentHealth));

        //Damaged or healed
        if (health > currentHealth)
        {
            
            health = currentHealth;

            if (healthPicCorIsRunning)
            {
                StopCoroutine(healthPicCoroutine);
            }
            //Set pic and bg
            if (currentHealth < healthPicChangeThreshhold)
            {
                healthBG.sprite = healthBGs[1];
                healthPic.sprite = healthPics[1];
                healthSlider.color = lowHealthBar;
            }
            else
            {
                
                healthPicCoroutine = StartCoroutine(HealthPicAnimation());

            }
        }
        else
        {
            if (currentHealth < healthPicChangeThreshhold)
            {
                healthBG.sprite = healthBGs[1];
                healthPic.sprite = healthPics[1];
                healthSlider.color = lowHealthBar;
            }
            else
            {
                healthBG.sprite = healthBGs[0];
                healthPic.sprite = healthPics[0];
                healthSlider.color = Color.white;
            }
        }

    }

    private IEnumerator RedHealthAnimation(float currentHealth)
    {
        print(currentHealth);
        redHealthCorIsRunning = true;
        yield return new WaitForSeconds(healthAnimTime);

        float t = 0;
        while (healthSliderRed.fillAmount != healthSlider.fillAmount)
        {
            t += Time.deltaTime;
            float relativeRedHealth = Mathf.Lerp(redHealth / 100, currentHealth / 100, redHealthAnimCurve.Evaluate(t * redHealthAnimSpeed));
            healthSliderRed.fillAmount = relativeRedHealth;
            redHealth = relativeRedHealth * 100;
            yield return null;
        }
        redHealthCorIsRunning = true;
    }

    IEnumerator HealthPicAnimation()
    {
        healthPicCorIsRunning = true;
        healthBG.sprite = healthBGs[1];
        healthPic.sprite = healthPics[2];
        yield return new WaitForSeconds(healthAnimTime);
        healthBG.sprite = healthBGs[0];
        healthPic.sprite = healthPics[0];
        healthSlider.color = Color.white;
        healthPicCorIsRunning = false;

    }

    public void UpdateDash(float dashAmount)
    {
        if((dashAmount > 1 && dashes[0].enabled == false) || (dashAmount == 2 && dashes[1].enabled == false)) {
            sfx.DashRecovered();
        }

        if (dashAmount < 1)
        {
            dashes[0].enabled = false;
        }
        if(dashAmount < 2)
        {
            dashes[1].enabled = false;
        }
        if(dashAmount > 1)
        {
            dashes[0].enabled = true;
        }
        if (dashAmount == 2) 
        {
            dashes[1].enabled = true;
        }
    }

    public void UpdateLife(int LifeAmount)
    {
        lifes.text = (LifeAmount.ToString() +"X");
    }

    public void UpdateGun(float gunCharge)
    {
        if((gunCharge >= 1 && gunCharges[0].enabled == false) || (gunCharge >= 2 && gunCharges[1].enabled == false) || (gunCharge >= 3 && gunCharges[2].enabled == false) || (gunCharge == 4 && gunCharges[3].enabled == false))
        {
            sfx.ShotRecovered();
        }
        if (gunCharge >= 1) { gunCharges[0].enabled = true; }
        else { gunCharges[0].enabled = false; }
        if (gunCharge >= 2) { gunCharges[1].enabled = true; }
        else { gunCharges[1].enabled = false; }
        if (gunCharge >= 3) { gunCharges[2].enabled = true; }
        else { gunCharges[2].enabled = false; }
        if (gunCharge >= 4) { gunCharges[3].enabled = true; }
        else { gunCharges[3].enabled = false; }

        //Slider
        float relativeGunCharge = gunCharge / 4;
        gunSlider.fillAmount = relativeGunCharge;
    }

    public void UpdatePlatform(float platformCharge)
    {
        if (platformCharge == 1) { platformChargeSprite.enabled = true; }
        else { platformChargeSprite.enabled = false; };

        //Slider
        platformSlider.fillAmount = platformCharge;
    }

    public void UpdateScore(int newPoints, int newScore)
    {
        score.text = newScore.ToString("0000000");
        scoreBG.text = newScore.ToString("0000000");
        var newPointsObj = Instantiate(newPointsPrefab, verticalLayerGroup.transform);
        newPointsObj.GetComponent<TextMeshProUGUI>().text = newPoints.ToString();

        StartCoroutine(ShowPoints(newPointsObj));
    }

    IEnumerator ShowPoints(GameObject newPoints)
    {
        yield return new WaitForSeconds(pointsTime);
        Destroy(newPoints);
    }

    public void UpdateMultiplier(int multiplier)
    {
        scoreMultiplier.text = multiplier.ToString();
    }

    public void UpdateObjectives(string mainObjective, string bonusObjective, int bonusPointsAmount)
    {
        objectiveMain.text = mainObjective;
        Debug.Log(mainObjective);
        objectiveBonus.text = bonusObjective;
        bonusPoints.text = "+" + bonusPointsAmount.ToString();
    }

    #endregion

    public void DebugRedHealth(float dmg)
    {
        float newHealth = Mathf.Clamp(health - dmg, 0, 100);
        UpdateHealth(newHealth);
    }
}
