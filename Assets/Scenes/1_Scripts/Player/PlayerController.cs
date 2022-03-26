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
    public Vector2 movement;

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
