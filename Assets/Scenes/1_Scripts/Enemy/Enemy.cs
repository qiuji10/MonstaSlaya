using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyData
{
    protected override void Awake()
    {
        currenthealth = maxHealth;
        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TimerAndDirectionRandomize(ref float directionChangeTime, ref Vector2 movementDirection, ref Vector2 movementPerSecond, ref float characterVelocity)
    {
        //random direction change time
        directionChangeTime = Random.Range(1, 4);

        //random direction
        if (transform.position.x > target.position.x)
        {
            if (transform.position.y > target.position.y)
                movementDirection = new Vector2(Random.Range(-1.0f, 0.1f), Random.Range(-1.0f, 0.1f)).normalized;
            else
                movementDirection = new Vector2(Random.Range(-1.0f, 0.1f), Random.Range(0.1f, 1.0f)).normalized;
        }
        else
        {
            if (transform.position.y > target.position.y)
                movementDirection = new Vector2(Random.Range(1.0f, -0.1f), Random.Range(-1.0f, 0.1f)).normalized;
            else
                movementDirection = new Vector2(Random.Range(1.0f, -0.1f), Random.Range(0.1f, 1.0f)).normalized;
        }

        //random counter for enemy will move or not
        int isMoving = Random.Range(-2, 10);

        if (isMoving > 0)
            movementPerSecond = movementDirection * characterVelocity;
        else
            movementPerSecond = new Vector2(0, 0);
    }
}
