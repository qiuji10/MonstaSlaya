using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyStats stats;

    public EnemyStats Stats
    {
        get => stats;
        set => stats = value;
    }

    void Start()
    {
        stats.currenthealth = stats.maxHealth;
    }

    void Update()
    {
        
    }
}
