using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    float maxATKTime = 4;
    float maxRestTime;

    public Enemy enemy;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
        maxRestTime = Random.Range(3, 8);

        if (enemy.enemyState == EnemyData.EnemyState.REST)
        {
            enemy.latestDirectionChangeTime = 0f;
            enemy.TimerAndDirectionRandomize();
        }
    }

    void Update()
    {
        enemy.FlipScale();
        enemy.AttackWarning();
    }

    void FixedUpdate()
    {
        if (enemy.enemyState == EnemyData.EnemyState.ATTACK)
        {
            enemy.AttackCounter(maxATKTime);

            enemy.FacingTarget();

            if (Vector2.Distance(transform.position, enemy.target.position) > enemy.minDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, enemy.target.position, enemy.speed * Time.deltaTime);
                //rb.MovePosition(Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime));
            }
            else
            {
                //attack
                enemy.animator.SetTrigger("WolfAttack");
                maxRestTime = Random.Range(5, 10);
                enemy.enemyState = EnemyData.EnemyState.REST;
            }
        }
        
        if (enemy.enemyState == EnemyData.EnemyState.REST)
        {
            enemy.RestMovement(maxRestTime);
        }
    }
}
