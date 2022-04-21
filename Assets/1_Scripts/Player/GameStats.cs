using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Menu/GameStatsData")]
public class GameStats : ScriptableObject
{
    public PlayerCore.Character state;
    public string time;
    public float milliseconds, seconds, minutes;
}