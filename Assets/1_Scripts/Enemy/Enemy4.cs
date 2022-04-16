using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4 : MonoBehaviour
{
    float maxAttackTime = 5;
    float maxRestTime;
    private bool shooted;

    public EnemyBase enemy;
    public GameObject enemyBullet;
    public Transform aimDirection;
    EnemyStates enemyStates;

    void Awake()
    {
        enemy = GetComponent<EnemyBase>();
        enemyStates = GetComponent<EnemyStates>();
        maxRestTime = Random.Range(3, 8);
        enemyStates.enemyState = EnemyStates.EnemyState.ATTACK;
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

            if (Vector2.Distance(transform.position, enemy.target.position) < enemy.minDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, enemy.target.position, -(enemy.speed + 5) * Time.deltaTime);
                //rb.MovePosition(Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime));
            }
            else
            {
                if (!shooted)
                {
                    Vector3 targetDirection = (enemy.target.position - aimDirection.position).normalized;
                    targetDirection = Quaternion.Euler(0, 0, 50) * targetDirection;
                    for (int i = 0; i < 4; i++)
                    {
                        GameObject bullet = Instantiate(enemyBullet, aimDirection.position, Quaternion.identity);
                        targetDirection = Quaternion.Euler(0, 0, -20) * targetDirection;
                        bullet.GetComponent<EnemyBulletBounce>().direction = targetDirection;
                    }

                    maxRestTime = Random.Range(5, 10);
                    shooted = true;
                    enemyStates.enemyState = EnemyStates.EnemyState.REST;
                }

            }
        }



        if (enemyStates.enemyState == EnemyStates.EnemyState.REST)
        {
            if (shooted)
                shooted = false;

            enemy.RestMovement(maxRestTime, enemyStates);
        }
    }
}
