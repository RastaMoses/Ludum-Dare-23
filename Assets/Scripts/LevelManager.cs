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
    Coroutine buttonResetCor;
    public bool loadTitle;

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
            StartCoroutine(WinLevel());
            
        }
        else
        {
            currentObjective++;
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

}
