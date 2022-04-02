using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : EnemyData
{
    [Header("Movement")]
    public float minDistance;
    public float latestDirectionChangeTime;
    public float directionChangeTime;
    public float characterVelocity = 2.5f;
    public float stateTimeCounter;
    public bool warning;
    [Space(20)]

    public Transform target;
    public GameObject warningPrefab;
    private Rigidbody2D rb;
    private Animator animator;

    public Rigidbody2D Rb
    {
        get => rb;
        set => rb = value;
    }

    public Animator Anim
    {
        get => animator;
        set => animator = value;
    }

    private Vector2 movementDirection;
    private Vector2 movementPerSecond;

    void Awake()
    {
        currenthealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        target = GameObject.Find("Player").GetComponent<Transform>();
    }

    public void TimerAndDirectionRandomize()
    {
        //random direction change time
        directionChangeTime = Random.Range(1, 4);

        //random direction, but will near player
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

    public void WalkAnimation()
    {
        if (movementPerSecond != Vector2.zero)
            animator.SetBool("isWalking", true);
        else
            animator.SetBool("isWalking", false);
    }

    public void AttackWarning(EnemyStates es)
    {
        if (!warning)
        {
            if (es.enemyState == EnemyStates.EnemyState.ATTACK)
            {
                GameObject warn = Instantiate(warningPrefab, new Vector3(transform.position.x + 1, transform.position.y + 1, transform.position.z), Quaternion.identity, transform);
                warning = true;

                Destroy(warn, 0.4f);
            }
        }
    }

    public void RestMovement(float maxRestTime, EnemyStates es)
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
           es.enemyState = EnemyStates.EnemyState.ATTACK;
        }

        //if the changeTime was reached, calculate a new movement vector
        if (Time.time - latestDirectionChangeTime > directionChangeTime)
        {
            latestDirectionChangeTime = Time.time;
            TimerAndDirectionRandomize();
        }

        rb.MovePosition(new Vector2(transform.position.x + (movementPerSecond.x * Time.deltaTime), transform.position.y + (movementPerSecond.y * Time.deltaTime)));
    }

    public void RestMovement()
    {
        if (warning)
            warning = false;

        if (movementDirection.x < 0)
            gameObject.transform.localScale = new Vector3(-1.05f, 1.05f, 0);
        else if (movementDirection.x > 0)
            gameObject.transform.localScale = new Vector3(1.05f, 1.05f, 0);

        //if the changeTime was reached, calculate a new movement vector
        if (Time.time - latestDirectionChangeTime > directionChangeTime)
        {
            latestDirectionChangeTime = Time.time;
            TimerAndDirectionRandomize();
        }

        rb.MovePosition(new Vector2(transform.position.x + (movementPerSecond.x * Time.deltaTime), transform.position.y + (movementPerSecond.y * Time.deltaTime)));
    }

    public void AttackCounter(float maxAtkTime, EnemyStates es)
    {
        stateTimeCounter += Time.deltaTime;
        if (stateTimeCounter >= maxAtkTime)
        {
            stateTimeCounter = 0;
            es.enemyState = EnemyStates.EnemyState.REST;
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
