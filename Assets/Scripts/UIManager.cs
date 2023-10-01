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
    [SerializeField] Image platformCharge;
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
    [SerializeField] Image healthBG;
    [SerializeField] List<Sprite> healthBGs;
    [SerializeField] Image healthPic;
    [SerializeField] List<Sprite> healthPics;
    [SerializeField] Image healthSlider;
    [SerializeField] Image lifes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
