using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    //Serialize Params
    [SerializeField] public float multiplierResetTime = 5f;
    [SerializeField] int maxMultiplier = 10;

    //Cached Comps
    UIManager ui;
    //State
    int currentScore;
    int multiplier = 1;
    float multiplierTimer;

    private void Awake()
    {
        ui = FindObjectOfType<UIManager>();

        multiplierTimer = multiplierResetTime;
    }

    private void Start()
    {
        ui.UpdateScore(0, 0);
        ui.UpdateMultiplier(1);
    }


    public void IncreaseScore(int amount)
    {
        currentScore += amount * multiplier;
        ui.UpdateScore(amount, currentScore);
    }

    public void IncreaseMultiplier(int amount)
    {
        //Input negative for losing Multiplier
        multiplier += amount;
        multiplier = Mathf.Clamp(multiplier, 1, maxMultiplier);
        ui.UpdateMultiplier(multiplier);
        if (amount > 0)
        {
            multiplierTimer = multiplierResetTime;
        }
        else
        {
            //Lose Multiplier Animation
        }
    }

    private void Update()
    {
        //Multiplier Reset Timer
        if (multiplier > 1)
        {
            if (multiplierTimer > 0)
            {
                multiplierTimer -= Time.deltaTime;
            }
            else
            {
                multiplier--;
                multiplierTimer = multiplierResetTime;
                ui.UpdateMultiplier(multiplier);
            }
            ui.UpdateMultiplierRing((multiplierResetTime - multiplierTimer) / multiplierResetTime);
        }
        else if (multiplierTimer != multiplierResetTime)
        {
            multiplierTimer = multiplierResetTime;
            ui.UpdateMultiplierRing((multiplierResetTime - multiplierTimer) / multiplierResetTime);
        }
    }
}
