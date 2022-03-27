using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyData
{

    void Start()
    {
        currenthealth = maxHealth;
        target = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
