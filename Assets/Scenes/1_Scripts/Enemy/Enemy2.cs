using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    float maxAttackTime = 4;
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
            enemy.AttackCounter(maxAttackTime);

            enemy.FacingTarget();

            if (Vector2.Distance(transform.position, enemy.target.position) > enemy.minDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, enemy.target.position, (enemy.speed + 2) * Time.deltaTime);
                //rb.MovePosition(Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime));
            }
            else
            {
                //attack
                enemy.animator.SetTrigger("TrollAttack");
                maxRestTime = Random.Range(5, 15);
                enemy.enemyState = EnemyData.EnemyState.REST;
            }
        }

        if (enemy.enemyState == EnemyData.EnemyState.REST)
        {
            enemy.RestMovement(maxRestTime);
        }
    }
}
