using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData : MonoBehaviour
{
    public int maxHealth;
    public int currenthealth;
    public int damage;
    public float speed;

    public float minDistance;
    public float latestDirectionChangeTime;
    public float directionChangeTime;
    public float characterVelocity = 2.5f;
    public float stateTimeCounter;
    public bool warning;

    public Transform target;

    public enum EnemyState { REST, ATTACK }
    public EnemyState enemyState = EnemyState.REST;

    protected virtual void Awake()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();
    }

    public void TakeDamage(int damaged)
    {
        Debug.Log("Enemy Hitted");
        currenthealth -= damaged;
        
        if (currenthealth <= 0)
        {
            Debug.Log("EnemyDie");
        }
    }
}