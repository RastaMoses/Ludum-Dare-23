using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //Serialize Params
    [SerializeField] List<Objective> objectives;
    [SerializeField] float timeBetweenObjectives;

    [Header("Objectives")]
    [Header("Buttons")]
    [SerializeField] float buttonCountdown = 10f;
    //[SerializeField] List<Button> buttons;

    //State
    int currentObjective = 0;
    int buttonIndex = 0;

    //Components
    UIManager ui;

    private void Awake()
    {
        ui = FindObjectOfType<UIManager>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StartObjective()
    {
        
        yield return new WaitForSeconds(timeBetweenObjectives);
        ui.UpdateObjectives(objectives[currentObjective].objectiveText, objectives[currentObjective].bonusObjectiveText, objectives[currentObjective].bonusObjectivePoints);
    }

    IEnumerator NextObjective()
    {
        yield return new WaitForSeconds(timeBetweenObjectives);
        currentObjective++;
    }

    #region Buttons

    //IEnumerator Button

    #endregion

}
