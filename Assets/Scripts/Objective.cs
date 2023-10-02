using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Objective : ScriptableObject
{
    public int waveToSpawn;
    public string objectiveText;
    public string bonusObjectiveText;
    public int bonusObjectivePoints;

    public bool buttonsObjective;
}
