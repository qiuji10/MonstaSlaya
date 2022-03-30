using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4 : MonoBehaviour
{
    public enum EnemyState { REST, ATTACK }

    [SerializeField] float minDistance;
    private float latestDirectionChangeTime;
    private float directionChangeTime;
    private float characterVelocity = 2.5f;
    public float stateTime;
    public float maxStateTime;
    private bool warning, shooted;

    public Enemy enemy;
    public GameObject warningPrefab, enemyBullet;
    public Transform aimDirection;
    Rigidbody2D rb;
    Animator animator;

    private Vector2 movementDirection;
    private Vector2 movementPerSecond;

    public EnemyState enemyState = EnemyState.ATTACK;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
    }

    void Start()
    {
        maxStateTime = Random.Range(3, 8);

        if (enemyState == EnemyState.REST)
        {
            latestDirectionChangeTime = 0f;
            enemy.TimerAndDirectionRandomize(ref directionChangeTime, ref movementDirection, ref movementPerSecond, ref characterVelocity);
        }
    }

    void Update()
    {
        if (movementPerSecond != Vector2.zero)
            animator.SetBool("isWalking", true);
        else
            animator.SetBool("isWalking", false);

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

    void FixedUpdate()
    {
        if (enemyState == EnemyState.ATTACK)
        {
            stateTime += Time.deltaTime;
            if (stateTime >= 5)
            {
                stateTime = 0;
                enemyState = EnemyState.REST;
            }

            if (movementDirection.x < enemy.target.position.x)
                gameObject.transform.localScale = new Vector3(1.05f, 1.05f, 0);
            else if (movementDirection.x > enemy.target.position.x)
                gameObject.transform.localScale = new Vector3(-1.05f, 1.05f, 0);

            if (Vector2.Distance(transform.position, enemy.target.position) < minDistance)
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

                    maxStateTime = Random.Range(5, 10);
                    shooted = true;
                    enemyState = EnemyState.REST;
                }

            }
        }



        if (enemyState == EnemyState.REST)
        {
            if (warning)
                warning = false;
            if (shooted)
                shooted = false;

            if (movementDirection.x < 0)
                gameObject.transform.localScale = new Vector3(-1.05f, 1.05f, 0);
            else if (movementDirection.x > 0)
                gameObject.transform.localScale = new Vector3(1.05f, 1.05f, 0);

            stateTime += Time.deltaTime;
            if (stateTime >= maxStateTime)
            {
                stateTime = 0;
                maxStateTime = Random.Range(3, 8);
                enemyState = EnemyState.ATTACK;
            }

            //if the changeTime was reached, calculate a new movement vector
            if (Time.time - latestDirectionChangeTime > directionChangeTime)
            {
                latestDirectionChangeTime = Time.time;
                enemy.TimerAndDirectionRandomize(ref directionChangeTime, ref movementDirection, ref movementPerSecond, ref characterVelocity);
            }

            rb.MovePosition(new Vector2(transform.position.x + (movementPerSecond.x * Time.deltaTime), transform.position.y + (movementPerSecond.y * Time.deltaTime)));
        }
    }
}
