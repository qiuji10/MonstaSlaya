using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyData
{
    public float minDistance;
    public float latestDirectionChangeTime;
    public float directionChangeTime;
    public float characterVelocity = 2.5f;
    public float stateTimeCounter;
    public bool warning;

    public GameObject warningPrefab;
    public Rigidbody2D rb;
    public Animator animator;

    private Vector2 movementDirection;
    private Vector2 movementPerSecond;

    protected override void Awake()
    {
        currenthealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TimerAndDirectionRandomize()
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

    public void FlipScale()
    {
        if (movementPerSecond != Vector2.zero)
            animator.SetBool("isWalking", true);
        else
            animator.SetBool("isWalking", false);
    }

    public void AttackWarning()
    {
        if (!warning)
        {
            if (enemyState == EnemyState.ATTACK)
            {
                GameObject warn = Instantiate(warningPrefab, new Vector3(transform.position.x + 1, transform.position.y + 1, transform.position.z), Quaternion.identity, transform);
                warning = true;

                Destroy(warn, 0.4f);
            }
        }
    }

    public void RestMovement(float maxRestTime)
    {
        if (warning)
            warning = false;

        if (movementDirection.x < 0)
            gameObject.transform.localScale = new Vector3(-1.05f, 1.05f, 0);
        else if (movementDirection.x > 0)
            gameObject.transform.localScale = new Vector3(1.05f, 1.05f, 0);

        stateTimeCounter += Time.deltaTime;
        if (stateTimeCounter >= maxRestTime)
        {
            stateTimeCounter = 0;
            enemyState = EnemyState.ATTACK;
        }

        //if the changeTime was reached, calculate a new movement vector
        if (Time.time - latestDirectionChangeTime > directionChangeTime)
        {
            latestDirectionChangeTime = Time.time;
            TimerAndDirectionRandomize();
        }

        rb.MovePosition(new Vector2(transform.position.x + (movementPerSecond.x * Time.deltaTime), transform.position.y + (movementPerSecond.y * Time.deltaTime)));
    }

    public void AttackCounter(float maxAtkTime)
    {
        stateTimeCounter += Time.deltaTime;
        if (stateTimeCounter >= maxAtkTime)
        {
            stateTimeCounter = 0;
            enemyState = EnemyState.REST;
        }
    }

    public void FacingTarget()
    {
        if (movementDirection.x < target.position.x)
            gameObject.transform.localScale = new Vector3(1.05f, 1.05f, 0);
        else if (movementDirection.x > target.position.x)
            gameObject.transform.localScale = new Vector3(-1.05f, 1.05f, 0);
    }
}
