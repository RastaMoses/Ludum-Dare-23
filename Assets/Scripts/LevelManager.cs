using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //Serialize Params

    [Header("Objectives")]
    [SerializeField] List<Objective> objectives;
    [SerializeField] float timeBetweenObjectives;
    [Header("Buttons")]
    [SerializeField] float buttonCountdown = 10f;
    [SerializeField] List<LevelButton> buttons;

    [Header("Enemies")]
    [SerializeField] List<GameObject> waves;

    //State
    int currentObjective = 0;
    int buttonsActive = 0;
    float buttonTimer;
    bool buttonsCounting;

    public int enemyCounter = 0;
    //Components
    UIManager ui;

    private void Awake()
    {
        ui = FindObjectOfType<UIManager>();
    }


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartObjective());
    }

    // Update is called once per frame
    void Update()
    {
        //Buttons Countdown
        if (buttonsCounting)
        {
            buttonTimer += Time.deltaTime;
            if (buttonTimer >= buttonCountdown)
            {
                foreach (var button in buttons)
                {
                    button.ResetButton();
                }

                buttonsActive = 0;
                buttonTimer = 0;
                buttonsCounting = false;
            }
        }

    }

    IEnumerator StartObjective()
    {

        yield return new WaitForSeconds(timeBetweenObjectives);

        //Sound
        //Visuals
        ui.UpdateObjectives(objectives[currentObjective].objectiveText, objectives[currentObjective].bonusObjectiveText, objectives[currentObjective].bonusObjectivePoints);
        if (objectives[currentObjective].buttonsObjective)
        {
            foreach(var button in buttons) { button.Activate(); }
        }
        if (objectives[currentObjective].spawnWave)
        {
            waves[objectives[currentObjective].waveToSpawn].SetActive(true);
            enemyCounter = waves[objectives[currentObjective].waveToSpawn].GetComponentsInChildren<HP>(false).Length;
        }
        
    }

    void NextObjective()
    {
        //Award Points
        //Set Visuals
        //Win Sound
        if (currentObjective == objectives.Count-1) 
        {
            //Win Level
            Debug.Log("Win Level");
        }
        else
        {
            currentObjective++;
            StartCoroutine(StartObjective());
        }
    }

    #region Buttons
    
    public void ButtonClick()
    {
        buttonTimer = 0;
        buttonsCounting = true;
        buttonsActive++;
        if (buttonsActive == buttons.Count)
        {
            foreach(var button in buttons)
            {
                button.Deactivate();
            }
            //all buttons activated
            NextObjective();
            buttonsCounting = false;
            buttonsActive = 0;
        }
    }
    #endregion

    #region Enemies

    public void EnemyKilled()
    {
        if (objectives[currentObjective].killAll)
        {
            enemyCounter--;
            if (enemyCounter == 0)
            {
                NextObjective();
            }
        }
    }

    #endregion

}
