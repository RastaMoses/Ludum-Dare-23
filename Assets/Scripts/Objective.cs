using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Objective : ScriptableObject
{
    public bool killAll;
    public bool spawnWave;
    public int waveToSpawn;
    public string objectiveText;
    public string bonusObjectiveText;
    public int bonusObjectivePoints;

    public bool buttonsObjective;
    public bool timeLimit;
    public float time;
    public bool noDamage;
}
