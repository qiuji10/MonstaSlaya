using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStates : MonoBehaviour
{
    public enum EnemyState { REST, ATTACK }
    public EnemyState enemyState = EnemyState.REST;
}
