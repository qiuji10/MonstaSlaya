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