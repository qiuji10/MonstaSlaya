using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float knightAtkRange = 1.2f;
    public float knightAtkRate = 2f;
    public float knightNxtAtk = 0f;

    public Transform knightAtkPoint;
    Rigidbody2D rb;
    Animator animator;

    public LayerMask enemyLayers;
    Vector2 movement;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.I))
        {
            SwitchCharacter();
        }

        if (Time.time >= knightNxtAtk && animator.GetBool("becomeKnight"))
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                KnightAttack();
                knightNxtAtk = Time.time + 1f / knightAtkRate;
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
        rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);
    }

    public void SwitchCharacter()
    {
        if (animator.GetBool("becomeKnight"))
        {
            animator.SetBool("becomeArcher", true);
            animator.SetBool("becomeKnight", false);
        }
        else if (animator.GetBool("becomeArcher"))
        {
            animator.SetBool("becomeAssassin", true);
            animator.SetBool("becomeArcher", false);
        }
        else if (animator.GetBool("becomeAssassin"))
        {
            animator.SetBool("becomeKnight", true);
            animator.SetBool("becomeAssassin", false);
        }
    }

    void KnightAttack()
    {
        animator.SetTrigger("KnightAttack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(knightAtkPoint.position, knightAtkRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hitting enemy");
            //enemy.GetComponent<Enemy>().TakeDamage(knightDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (knightAtkPoint == null)
            return;

        Gizmos.DrawWireSphere(knightAtkPoint.position, knightAtkRange);
    }
}
