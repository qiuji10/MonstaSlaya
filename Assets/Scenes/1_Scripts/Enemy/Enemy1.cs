using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public enum EnemyState { REST, ATTACK }

    public float attackRange;
    [SerializeField] float minDistance;
    private float latestDirectionChangeTime;
    private float directionChangeTime;
    private float characterVelocity = 2.5f;
    public float restTime;
    public float maxRestTime;

    public Transform attackPoint;
    public LayerMask playerLayer;
    public Enemy enemy;
    Rigidbody2D rb;
    Animator animator;
    

    private Vector2 movementDirection;
    private Vector2 movementPerSecond;

    public EnemyState enemyState = EnemyState.REST;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
    }

    void Start()
    {
        maxRestTime = Random.Range(3, 8);

        if (enemyState == EnemyState.REST)
        {
            latestDirectionChangeTime = 0f;
            TimerAndDirectionRandomize();
        }
    }

    void Update()
    {
        if (movementPerSecond != Vector2.zero)
            animator.SetBool("isWalking", true);
        else
            animator.SetBool("isWalking", false);
    }

    //Moving towards Player with a distance of 4 (Attack)
    void FixedUpdate()
    {
        if (enemyState == EnemyState.ATTACK)
        {
            restTime += Time.deltaTime;
            if (restTime >= 6)
            {
                restTime = 0;
                enemyState = EnemyState.REST;
            }

            if (movementDirection.x < enemy.target.position.x)
                gameObject.transform.localScale = new Vector3(1.05f, 1.05f, 0);
            else if (movementDirection.x > enemy.target.position.x)
                gameObject.transform.localScale = new Vector3(-1.05f, 1.05f, 0);

            if (Vector2.Distance(transform.position, enemy.target.position) > minDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, enemy.target.position, enemy.speed * Time.deltaTime);
                //rb.MovePosition(Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime));
            }
            else
            {
                //attack
                Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
                foreach (Collider2D player in hitPlayer)
                {
                    if (!player.isTrigger)
                    {
                        animator.SetTrigger("WolfAttack");

                        if (Vector2.Distance(attackPoint.position, enemy.target.position) < attackRange)
                        {
                            player.GetComponent<PlayerCore>().PlayerDamaged(enemy.damage);
                        }
                            
                    }

                    if (player.isTrigger)
                    {
                        continue;
                    }
                }

                maxRestTime = Random.Range(3, 8);
                enemyState = EnemyState.REST;
            }
        }
        


        if (enemyState == EnemyState.REST)
        {
            if (movementDirection.x < 0)
                gameObject.transform.localScale = new Vector3(-1.05f, 1.05f, 0);
            else if (movementDirection.x > 0)
                gameObject.transform.localScale = new Vector3(1.05f, 1.05f, 0);

            restTime += Time.deltaTime;
            if (restTime >= maxRestTime)
            {
                restTime = 0;
                enemyState = EnemyState.ATTACK;
            }

            //if the changeTime was reached, calculate a new movement vector
            if (Time.time - latestDirectionChangeTime > directionChangeTime)
            {
                latestDirectionChangeTime = Time.time;
                TimerAndDirectionRandomize();
            }

            rb.MovePosition(new Vector2(transform.position.x + (movementPerSecond.x * Time.deltaTime), transform.position.y + (movementPerSecond.y* Time.deltaTime)));
        }
    }

    void TimerAndDirectionRandomize()
    {
        //random direction change time
        directionChangeTime = Random.Range(1, 4);

        //random direction
        if (transform.position.x > enemy.target.position.x)
        {
            if (transform.position.y > enemy.target.position.y)
                movementDirection = new Vector2(Random.Range(-1.0f, 0.1f), Random.Range(-1.0f, 0.1f)).normalized;
            else
                movementDirection = new Vector2(Random.Range(-1.0f, 0.1f), Random.Range(0.1f, 1.0f)).normalized;
        }
        else
        {
            if (transform.position.y > enemy.target.position.y)
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

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
