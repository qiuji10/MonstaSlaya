using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    float maxAttackTime = 4;
    float maxRestTime;

    public EnemyBase enemy;
    EnemyStates enemyStates;

    void Awake()
    {
        enemy = GetComponent<EnemyBase>();
        enemyStates = GetComponent<EnemyStates>();
        maxRestTime = Random.Range(5, 15);

    }

    private void Start()
    {
        if (enemyStates.enemyState == EnemyStates.EnemyState.REST)
        {
            enemy.latestDirectionChangeTime = 0f;
            enemy.TimerAndDirectionRandomize();
        }
    }

    void Update()
    {
        enemy.WalkAnimation();
        enemy.AttackWarning(enemyStates);
    }

    void FixedUpdate()
    {
        if (enemyStates.enemyState == EnemyStates.EnemyState.ATTACK)
        {
            enemy.AttackCounter(maxAttackTime, enemyStates);

            enemy.FacingTarget();

            if (Vector2.Distance(transform.position, enemy.target.position) > enemy.minDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, enemy.target.position, (enemy.speed + 2) * Time.deltaTime);
                //rb.MovePosition(Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime));
            }
            else
            {
                //attack
                enemy.Anim.SetTrigger("TrollAttack");
                maxRestTime = Random.Range(5, 15);
                enemyStates.enemyState = EnemyStates.EnemyState.REST;
            }
        }

        if (enemyStates.enemyState == EnemyStates.EnemyState.REST)
        {
            enemy.RestMovement(maxRestTime, enemyStates);
        }
    }
}
