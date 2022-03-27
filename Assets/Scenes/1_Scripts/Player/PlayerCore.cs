using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    public enum Character { KNIGHT, ARCHER, ASSASSIN };

    public bool enemyInRange;
    public float speed = 5f;

    public ClosestEnemy closestEnemy;
    Animator animator;
    PlayerController playerController;

    [Header("Knight")]
    public Transform knightAtkPoint;
    public float knightAtkRate = 0.5f;
    public float knightAtkCD = 0f;
    public int knightDamage = 1;
    public float knightAtkRange = 1.2f;
    [Space(20)]

    [Header("Archer")]
    public Transform archerAim;
    public GameObject arrow;
    public float archerAtkRate = 0.8f;
    public float archerAtkCD = 0f;
    Vector3 archerAimDirection, facingDirection;
    [Space(20)]

    [Header("Assassin")]
    public Transform assassinAtkPoint;
    public int assassinDamage = 1;
    public float assassinAtkRange = 1f;
    [Space(20)]

    public Character playerState = Character.KNIGHT;
    public LayerMask enemyLayers;
    Vector3 enemyPos;
    Vector3 facingRight = new Vector3(1.5f, 1.5f, 1);
    Vector3 facingLeft = new Vector3(-1.5f, 1.5f, 1);

    void Awake()
    {
        animator = GetComponent<Animator>();
        closestEnemy = GetComponent<ClosestEnemy>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (enemyInRange)
        {
            closestEnemy.FindClosestEnemy(ref enemyPos);
        }

        if (playerController.movement.x != 0 || playerController.movement.y != 0)
        {
            animator.SetBool("isWalking", true);
            facingDirection = new Vector2(playerController.movement.x, playerController.movement.y);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }


        if (enemyInRange)
        {
            if (enemyPos.x < transform.position.x)
            {
                gameObject.transform.localScale = facingLeft;
            }
            else if (enemyPos.x > transform.position.x)
            {
                gameObject.transform.localScale = facingRight;
            }
        }
        else
        {
            if (playerController.movement.x < 0)
            {
                gameObject.transform.localScale = facingLeft;
            }
            else if (playerController.movement.x > 0)
            {
                gameObject.transform.localScale = facingRight;
            }
        }

        if (playerState == Character.ARCHER)
        {
            if (enemyInRange)
            {
                archerAimDirection = (enemyPos - transform.position).normalized;

                float angle = Mathf.Atan2(archerAimDirection.y, archerAimDirection.x) * Mathf.Rad2Deg;

                if (enemyPos.x < transform.position.x)
                {
                    archerAim.eulerAngles = new Vector3(0, 0, angle - 180);
                }
                else if (enemyPos.x > transform.position.x)
                {
                    archerAim.eulerAngles = new Vector3(0, 0, angle);
                }
            }

            else
            {
                //rotate bow but not complete
                if ((playerState == Character.ARCHER))
                {
                    //float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
                    //if (playerController.movement.x > 0)
                    //{
                    //    archerAim.eulerAngles = new Vector3(0, 0, angle);
                    //}
                    //else if (playerController.movement.x < 0)
                    //{
                    //    archerAim.eulerAngles = new Vector3(0, 0, angle - 180);
                    //}
                    //else if (playerController.movement.y > 0)
                    //{
                    //    archerAim.eulerAngles = new Vector3(0, 0, 90);
                    //}
                    float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
                    if (playerController.movement.x > 0)
                    {
                        archerAim.transform.eulerAngles = Vector3.forward * angle;
                    }
                    else if (playerController.movement.x < 0)
                    {
                        angle += 180;
                        archerAim.transform.eulerAngles = Vector3.forward * angle;
                    }
                }
            }
        }
    }

    public void KnightAttack()
    {
        animator.SetTrigger("KnightAttack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(knightAtkPoint.position, knightAtkRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hitting enemy");
            enemy.GetComponent<Enemy>().Stats.TakeDamage(knightDamage);
        }
    }

    public void ArcherAttack()
    {
        animator.SetTrigger("ArcherAttack");

        Vector3 offset = new Vector3(archerAim.position.x - 100, archerAim.position.y, archerAim.position.z);
        GameObject arrow = Instantiate(this.arrow, offset, Quaternion.identity);
        arrow.transform.position = archerAim.position;

        if (enemyPos.x < transform.position.x && enemyInRange)
        {
            archerAim.rotation *= Quaternion.Euler(0, 0, -270);
        }
        else if (enemyPos.x > transform.position.x && enemyInRange)
        {
            archerAim.rotation *= Quaternion.Euler(0, 0, 270);
        }

        if (enemyInRange)
        {
            arrow.GetComponent<Arrow>().direction = archerAimDirection;
            arrow.transform.rotation = archerAim.rotation;
        }
        else
        {
            arrow.GetComponent<Arrow>().direction = facingDirection.normalized;
            float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        }        
    }

    public void AssassinAttack()
    {
        animator.SetTrigger("AssassinAttack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(assassinAtkPoint.position, assassinAtkRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hitting enemy");
            enemy.GetComponent<Enemy>().Stats.TakeDamage(assassinDamage);
        }
    }

    public void SwitchCharacter()
    {
        if (playerState == Character.KNIGHT)
        {
            playerState = Character.ARCHER;
            animator.SetBool("becomeArcher", true);
            animator.SetBool("becomeKnight", false);
        }
        else if (playerState == Character.ARCHER)
        {
            playerState = Character.ASSASSIN;
            animator.SetBool("becomeAssassin", true);
            animator.SetBool("becomeArcher", false);
        }
        else if (playerState == Character.ASSASSIN)
        {
            playerState = Character.KNIGHT;
            animator.SetBool("becomeKnight", true);
            animator.SetBool("becomeAssassin", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            enemyInRange = true;
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            enemyInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            enemyInRange = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (knightAtkPoint == null)
            return;

        if (assassinAtkPoint == null)
            return;

        Gizmos.DrawWireSphere(knightAtkPoint.position, knightAtkRange);
        Gizmos.DrawWireSphere(assassinAtkPoint.position, assassinAtkRange);
    }
}
