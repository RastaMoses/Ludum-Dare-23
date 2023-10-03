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

    [Header("Misc")]
    public bool loadTitle;

    //State
    //Objectives
    int currentObjective = 0;
    bool bonusObjectiveFailed = false;
    Coroutine bonusTimerCor;
    float timeLimitTimer;



    //Buttons
    int buttonsActive = 0;
    float buttonTimer;
    bool buttonsCounting;
    Coroutine buttonResetCor;
    
    //Enemy Wave
    [HideInInspector]public int enemyCounter = 0;
    
    
    //Components
    UIManager ui;
    FPS_Controller player;

    private void Awake()
    {
        ui = FindObjectOfType<UIManager>();
        player = FindObjectOfType<FPS_Controller>();
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
                buttonsCounting = false;
                buttonResetCor= StartCoroutine(ButtonReset());
                
            }
        }


    }

    IEnumerator StartObjective()
    {

        yield return new WaitForSeconds(timeBetweenObjectives);

        //Sound
        //Visuals
        ui.UpdateObjectives(objectives[currentObjective].objectiveText, objectives[currentObjective].bonusObjectiveText, objectives[currentObjective].bonusObjectivePoints);
        
        //Start Objective
        if (objectives[currentObjective].buttonsObjective)
        {
            ButtonObjective();
        }
        if (objectives[currentObjective].spawnWave)
        {
            WaveObjective();
        }

        //Start Bonus Objective
        bonusObjectiveFailed = false;

        if (objectives[currentObjective].timeLimit)
        {
            timeLimitTimer = objectives[currentObjective].time;
            bonusTimerCor = StartCoroutine(BonusTimer());
            ui.UpdateBonusTracker(timeLimitTimer.ToString("000"));
        }
        if (objectives[currentObjective].noDamage)
        {
            ui.UpdateBonusTracker("GOOD");
        }
        
    }

    void NextObjective()
    {
        //Award Points

        //Bonus Objectives
        //Time Limit
        if (objectives[currentObjective].timeLimit)
        {
            if (timeLimitTimer != 0)
            {
                BonusObjComplete();
                StopCoroutine(bonusTimerCor);
            }
        }
        if (objectives[currentObjective].noDamage)
        {
            if (!bonusObjectiveFailed)
            {
                BonusObjComplete();
            }
        }
        //Set Visuals
        //Win Sound
        if (currentObjective == objectives.Count-1) 
        {
            //If last objective of level
            StartCoroutine(WinLevel());
        }
        else
        {
            currentObjective++;
            //TimeLimit check
            
            StartCoroutine(StartObjective());
        }
        
    }

    IEnumerator WinLevel()
    {
        ui.ShowWin();
        yield return new WaitForSeconds(6);
        if (loadTitle) { FindObjectOfType<SceneLoader>().LoadTitle(); }
        else { FindObjectOfType<SceneLoader>().LoadNextLevel(); }

    }

    #region Buttons
    
    public void ButtonClick()
    {
        buttonTimer = 0;
        buttonsCounting = true;
        buttonsActive++;
        if (buttonResetCor != null)
        {
            StopCoroutine(buttonResetCor);
        }
        foreach (var button in  buttons)
        {
            if (button.pressed)
            {
                button.SetWaitColor();
            }
            else
            {
                button.SetActiveColor();
            }
        }
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

    IEnumerator ButtonReset()
    {
        yield return new WaitForSeconds(buttons[0].resetAnimDuration);
        buttonsActive = 0;
        buttonTimer = 0;
        buttonsCounting = false;
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

    #region Objectives
    private void ButtonObjective()
    {
        foreach (var button in buttons) { button.Activate(); }
    }

    private void WaveObjective()
    {
        waves[objectives[currentObjective].waveToSpawn].SetActive(true);
        enemyCounter = waves[objectives[currentObjective].waveToSpawn].GetComponentsInChildren<HP>(false).Length;
    }

    #endregion

    #region Bonus Objectives

    private IEnumerator BonusTimer()
    {
        while(timeLimitTimer > 0)
        {
            timeLimitTimer -= Time.deltaTime;
            timeLimitTimer = Mathf.Clamp(timeLimitTimer, 0, objectives[currentObjective].time);
            yield return null;
            ui.UpdateBonusTracker(timeLimitTimer.ToString("000"));
        }
        if (bonusObjectiveFailed)
        {
            BonusObjFailed();
        }
    }

    public void BonusNoDamage()
    {
        if (objectives[currentObjective].noDamage)
        {
            BonusObjFailed();
        }
    }



    private void BonusObjFailed()
    {
        bonusObjectiveFailed = true;
        ui.UpdateBonusTracker("FAILED");

        //UI
    }

    private void BonusObjComplete()
    {
        //Give Score
        player.GetComponent<Score>().IncreaseScore(objectives[currentObjective].bonusObjectivePoints, "Bonus");
        
        //UI
        
    }

    #endregion
}
