using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCore : MonoBehaviour
{
    public enum Character { KNIGHT, ARCHER, ASSASSIN };

    public float speed = 5f;

    Animator animator;
    PlayerController playerController;
    SpriteRenderer sp;
    CinemachineImpulseSource impSource;

    [Header("Knight")]
    public Transform knightAtkPoint;
    public float knightAtkRate = 0.5f;
    public float knightAtkCD = 0.5f;
    public int knightDamage = 1;
    public float knightAtkRange = 1.2f;
    [Space(20)]

    [Header("Archer")]
    public Transform archerAim;
    public GameObject arrow;
    public float archerAtkRate = 0.8f;
    public float archerAtkCD = 0f;
    Vector3 archerAimDirection;
    [Space(20)]

    [Header("Assassin")]
    public Transform assassinAtkPoint;
    public int assassinDamage = 1;
    public float assassinAtkRange = 1f;
    [Space(20)]

    public Character playerState = Character.KNIGHT;
    public LayerMask enemyLayers;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        sp = GetComponent<SpriteRenderer>();
        impSource = FindObjectOfType<CinemachineImpulseSource>();
    }

    void Update()
    {
        if (playerController.movement.x != 0 || playerController.movement.y != 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if (playerController.mousePos.x < transform.position.x)
        {
            sp.flipX = true;
        }
        else if (playerController.mousePos.x > transform.position.x)
        {
            sp.flipX = false;
        }
    }

    public void KnightAttack(int atkCombo)
    {
        if (atkCombo == 1)
            animator.SetTrigger("KnightAttack1");
        else if (atkCombo == 2)
            animator.SetTrigger("KnightAttack2");
        else if (atkCombo == 3)
            animator.SetTrigger("KnightAttack3");
        MeleeAttack(knightAtkPoint.position, knightAtkRange, knightDamage, atkCombo);
    }

    public void ArcherAttack()
    {
        animator.SetTrigger("ArcherAttack");
    }

    public void ArcherShoot(Vector3 mousePosition, int max, int min)
    {
        animator.ResetTrigger("ArcherAttack");
        animator.SetTrigger("ArcherShooted");
        archerAimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(archerAimDirection.y, archerAimDirection.x) * Mathf.Rad2Deg;
        archerAim.eulerAngles = new Vector3(0, 0, angle);

        //random shoot angle
        Quaternion randAngle = Quaternion.Euler(0, 0, Random.Range(min, max));

        archerAimDirection = randAngle * archerAimDirection;
        Vector3 offset = new Vector3(archerAim.position.x, archerAim.position.y, archerAim.position.z);
        GameObject arrow = Instantiate(this.arrow, offset, archerAim.rotation);
        arrow.transform.position = archerAim.position;
        arrow.GetComponent<Arrow>().direction = archerAimDirection;
        arrow.transform.rotation = randAngle * archerAim.rotation;
    }

    public void AssassinAttack()
    {
        animator.SetTrigger("AssassinAttack");
        MeleeAttack(assassinAtkPoint.position, assassinAtkRange, assassinDamage, 1);
    }

    public void MeleeAttack(Vector2 atkPoint, float atkRange, int damage, int combo)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(atkPoint, atkRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.GetComponent<EnemyBase>() != null && playerState == Character.KNIGHT && combo == 3)
            {
                impSource.GenerateImpulse();
                enemy.GetComponent<EnemyBase>().TakeDamage(damage + 3);
                continue;
            }

            if (enemy.GetComponent<EnemyBase>() != null)
                enemy.GetComponent<EnemyBase>().TakeDamage(damage);
            else
                Destroy(enemy.transform.parent.gameObject);
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

    public void PlayerDamaged(int damage)
    {
        Debug.Log("Hitting player");
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
