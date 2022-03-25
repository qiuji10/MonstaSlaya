using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyStats
{
    public int maxHealth;
    public int currenthealth;
    public int damage;
    public int speed;

    public void TakeDamage(int damage)
    {
        currenthealth -= damage;

        if (currenthealth <= 0)
        {
            Debug.Log("Enemy Die");
        }
    }
}