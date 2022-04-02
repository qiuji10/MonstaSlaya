using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int maxHealth;
    public int currenthealth;
    public int damage;
    public float speed;

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