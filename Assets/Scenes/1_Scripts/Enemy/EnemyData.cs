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

    public Transform target;

    public void TakeDamage(int damaged)
    {
        currenthealth -= damaged;
        
        if (currenthealth <= 0)
        {
            Debug.Log("EnemyDie");
        }
    }
}