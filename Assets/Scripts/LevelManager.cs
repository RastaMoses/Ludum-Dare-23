using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //Serialize Params
    [SerializeField] List<Objective> objectives;
    [SerializeField] float timeBetweenObjectives;

    //State
    int currentObjective = 0;

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

    }
}
