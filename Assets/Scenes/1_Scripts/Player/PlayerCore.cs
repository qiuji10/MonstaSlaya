using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    public enum Character { KNIGHT, ARCHER, ASSASSIN };

    public bool enemyInRange;
    public float speed = 5f;
    public float knightAtkRate = 2f;
    public float knightNxtAtk = 0f;
    public int knightDamage = 1;

    public ClosestEnemy closestEnemy;
    Animator animator;
    PlayerController playerController;

    public Character playerState = Character.KNIGHT;
    public Vector3 enemyPos;
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
}
