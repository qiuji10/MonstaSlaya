using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    public enum EnemyState { REST, ATTACK }

    [SerializeField] float minDistance;
    private float latestDirectionChangeTime;
    private float directionChangeTime;
    private float characterVelocity = 2.5f;
    public float stateTime;
    public float maxStateTime;

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
        enemy.target = GameObject.Find("Player").GetComponent<Transform>();
    }

    void Start()
    {
        maxStateTime = Random.Range(3, 8);

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

    void FixedUpdate()
    {
        if (enemyState == EnemyState.ATTACK)
        {
            stateTime += Time.deltaTime;
            if (stateTime >= 10)
            {
                stateTime = 0;
                enemyState = EnemyState.REST;
            }

            if (movementDirection.x < enemy.target.position.x)
                gameObject.transform.localScale = new Vector3(1.05f, 1.05f, 0);
            else if (movementDirection.x > enemy.target.position.x)
                gameObject.transform.localScale = new Vector3(-1.05f, 1.05f, 0);

            if (Vector2.Distance(transform.position, enemy.target.position) > minDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, enemy.target.position, (enemy.speed + 2) * Time.deltaTime);
                //rb.MovePosition(Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime));
            }
            else
            {
                //attack
                animator.SetTrigger("TrollAttack");
                maxStateTime = Random.Range(3, 8);
                enemyState = EnemyState.REST;
            }
        }
        


        if (enemyState == EnemyState.REST)
        {
            if (movementDirection.x < 0)
                gameObject.transform.localScale = new Vector3(-1.05f, 1.05f, 0);
            else if (movementDirection.x > 0)
                gameObject.transform.localScale = new Vector3(1.05f, 1.05f, 0);

            stateTime += Time.deltaTime;
            if (stateTime >= maxStateTime)
            {
                stateTime = 0;
                enemyState = EnemyState.ATTACK;
            }

            //if the changeTime was reached, calculate a new timer and movement vector
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

        //track player direction + random direction
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
}
