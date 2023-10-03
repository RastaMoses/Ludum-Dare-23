using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Objective : ScriptableObject
{
    [Header("Basics")]
    public string objectiveText;
    public string bonusObjectiveText;
    public int bonusObjectivePoints;

    [Header("Objective")]
    public bool buttonsObjective;
    public bool killAll;
    public bool spawnWave;
    public int waveToSpawn;

    [Header("Bonus Objectives")]
    public bool timeLimit;
    public float time;
    public bool noDamage;
}
