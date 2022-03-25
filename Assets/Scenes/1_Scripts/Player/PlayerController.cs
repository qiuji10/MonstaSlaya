using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float knightAtkRange = 1.2f;

    public Transform knightAtkPoint;
    Rigidbody2D rb;
    Animator animator;
    PlayerCore playerCore;

    public LayerMask enemyLayers;
    Vector2 movement;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCore = GetComponent<PlayerCore>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.I))
        {
            playerCore.SwitchCharacter();
        }

        if (Time.time >= playerCore.knightNxtAtk && playerCore.playerState == PlayerCore.Character.KNIGHT)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                KnightAttack();
                playerCore.knightNxtAtk = Time.time + 1f / playerCore.knightAtkRate;
            }
        }

        if (movement.x != 0 || movement.y != 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if (movement.x < 0)
        {
            gameObject.transform.localScale = new Vector3(-1.5f, 1.5f, 1);
        }
        else if (movement.x > 0)
        {
            gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * playerCore.speed * Time.fixedDeltaTime);
    }

    

    void KnightAttack()
    {
        animator.SetTrigger("KnightAttack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(knightAtkPoint.position, knightAtkRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hitting enemy");
            enemy.GetComponent<Enemy>().Stats.TakeDamage(playerCore.knightDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (knightAtkPoint == null)
            return;

        Gizmos.DrawWireSphere(knightAtkPoint.position, knightAtkRange);
    }
}
