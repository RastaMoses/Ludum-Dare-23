using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [Header("Reticle")]
    [SerializeField] Image crosshair;
    [SerializeField] List<Image> dashes;
    [SerializeField] Image platformChargeSprite;
    [SerializeField] Image platformSlider;
    [SerializeField] List<Image> gunCharges;
    [SerializeField] Image gunSlider;
    [Header("Score")]
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] TextMeshProUGUI scoreMultiplier;
    [SerializeField] List<TextMeshProUGUI> newPoints;
    [Header("Objectives")]
    [SerializeField] TextMeshProUGUI objectiveMain;
    [SerializeField] TextMeshProUGUI objectiveBonus;
    [Header("Health")]
    [SerializeField] float debugHealth = 100;
    [SerializeField] float healthPicChangeThreshhold = 40;
    [SerializeField] Image healthBG;
    [SerializeField] List<Sprite> healthBGs;
    [SerializeField] Image healthPic;
    [SerializeField] List<Sprite> healthPics;
    [SerializeField] Image healthSlider;
    [SerializeField] Color lowHealthBar;
    [SerializeField] TextMeshProUGUI lifes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealth(debugHealth);
    }

    #region Public Update Visuals Functions
    public void UpdateHealth(float currentHealth)
    {
        //Set pic and bg
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

        //Slider
        float relativeHP = currentHealth / 100;
        healthSlider.fillAmount = relativeHP;

    }

    public void UpdateDash(float dashAmount)
    {
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

    #endregion
}
