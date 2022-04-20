using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCore : MonoBehaviour
{
    public enum Character { KNIGHT, ARCHER, ASSASSIN };

    [Header("Player")]
    public int maxHealth, currentHealth, maxShield, currentShield;
    public float speed = 10f, recoverTimePerSec = 3f, recoverTimer;
    public bool isParalyzed, statusChanged;
    public bool immunity;
    private float paralyzedTimer;

    Animator animator;
    PlayerController playerController;
    SpriteRenderer sp;
    CinemachineImpulseSource impSource;
    GameObject paralyzedSymbol;
    UIManager uiManager;

    [Header("Knight")]
    public Transform knightAtkPoint;
    public GameObject shield;
    public bool knightSkill;
    public float knightSkillTimer;
    public float knightCDTime = 20;
    public float knightAtkRate = 0.5f;
    public float knightAtkCD = 0.5f;
    public int knightDamage = 6;
    public float knightAtkRange = 1.2f;
    [Space(20)]

    [Header("Archer")]
    public Transform archerAim;
    public GameObject arrow, archerAOE, archerSkillCAM;
    public CinemachineVirtualCamera vcam1;
    public bool archerSkill;
    public float archerSkillTimer;
    public float archerCDTime = 30;
    public float archerAtkRate = 0.8f;
    public float archerAtkCD = 0f;
    Vector3 archerAimDirection;
    [Space(20)]

    [Header("Assassin")]
    public Transform assassinAtkPoint;
    public GameObject trail;
    public bool assassinSkill, assassinShowtime;
    public float assassinSkillTimer;
    public float assassinCDTime = 6;
    public float assassinGoForce;
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
        uiManager = FindObjectOfType<UIManager>();
        paralyzedSymbol = transform.Find("Paralyzed").gameObject;
        //vcam1 = transform.Find("ArcherSkill").GetComponentInChildren<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        currentShield = maxShield;
        uiManager.MaxHealthText.text = maxHealth.ToString();
        uiManager.MaxShieldText.text = maxShield.ToString();
        uiManager.CurrentHealthText.text = currentHealth.ToString();
        uiManager.CurrentShieldText.text = currentShield.ToString();
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

        if (isParalyzed)
        {
            speed = 0;
            paralyzedSymbol.SetActive(true);
            paralyzedTimer += Time.deltaTime;

            if (paralyzedTimer > 5)
            {
                paralyzedTimer = 0;
                paralyzedSymbol.SetActive(false);
                speed = 10;
                isParalyzed = false;
            }
        }

        if (knightSkill)
        {
            knightSkillTimer += Time.deltaTime;
            if (knightSkillTimer >= 5 || playerState != Character.KNIGHT)
            {
                immunity = false;
                shield.SetActive(false);
            }

            if (knightSkillTimer >= knightCDTime)
            {
                uiManager.KnightText.gameObject.SetActive(false);
                knightSkillTimer = 0;
                knightSkill = false;
            }
        }

        if (archerSkill)
        {
            archerSkillTimer += Time.deltaTime;
            if (archerSkillTimer >= archerCDTime)
            {
                uiManager.ArcherText.gameObject.SetActive(false);
                archerSkillTimer = 0;
                archerSkill = false;
            }
        }

        if (assassinSkill)
        {
            assassinSkillTimer += Time.deltaTime;
            if (assassinSkillTimer >= assassinCDTime)
            {
                uiManager.AssassinText.gameObject.SetActive(false);
                assassinSkillTimer = 0;
                assassinSkill = false;
            }
        }

        if (currentShield < maxShield && currentHealth > 0)
        {
            recoverTimer += 1.0f / recoverTimePerSec * Time.deltaTime;

            if (currentShield + 1 <= recoverTimer)
            {
                currentShield = Mathf.FloorToInt(recoverTimer);
                uiManager.CurrentShieldText.text = currentShield.ToString();
                statusChanged = true;
            }
        }
        else if (currentShield >= maxShield)
        {
            recoverTimer = 0;
        }
    }

    public void KnightAttack(int atkCombo)
    {
        if (atkCombo == 1 || atkCombo == 0)
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

    public void ArcherShoot(Vector3 mousePosition, int max, int min, int damage)
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
        arrow.GetComponent<Arrow>().dmg = damage;
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
                enemy.GetComponent<EnemyBase>().TakeDamage((damage + 2), transform.position);
                continue;
            }

            if (enemy.GetComponent<EnemyBase>() != null)
            {
                //Vector2 dist = transform.position - enemy.gameObject.transform.position;
                //Vector2 force = dist * 3;//knockbackForce
                enemy.GetComponent<EnemyBase>().TakeDamage(damage, transform.position);
                //enemy.GetComponent<Rigidbody2D>().AddForce(force * Time.fixedDeltaTime, ForceMode2D.Impulse);
            }
            else
                Destroy(enemy.transform.parent.gameObject);

            if (enemy.GetComponent<Boss_FSM>() != null)
            {
                enemy.GetComponent<Boss_FSM>().healthBar.BossHealthChange(damage);
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

    public void PlayerDamaged(int damage)
    {
        if (!immunity)
        {
            Debug.Log("Hitting player " + damage);
            if (currentShield > 0)
            {
                Debug.Log("0");
                currentShield -= damage;
                recoverTimer = currentShield;

                if (currentShield < 0 && currentHealth > 0)
                {
                    Debug.Log("1");
                    damage -= maxShield;
                    currentShield = 0;
                    uiManager.CurrentShieldText.text = currentShield.ToString();
                    currentHealth -= damage;
                    if (currentHealth < 0)
                    {
                        currentHealth = 0;
                    }
                    uiManager.CurrentHealthText.text = currentHealth.ToString();
                }
            }

            if (currentShield <= 0 && currentHealth > 0)
            {
                Debug.Log("2");
                currentHealth -= damage;
                if (currentHealth < 0)
                {
                    currentHealth = 0;
                }
            }

            statusChanged = true;
            uiManager.CurrentShieldText.text = currentShield.ToString();
            uiManager.CurrentHealthText.text = currentHealth.ToString();

            StartCoroutine(DamageSprite());

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Debug.Log("PlayerDie");
            }
        }
    }

    public IEnumerator Paralyzed()
    {
        playerController.enabled = false;
        yield return new WaitForSeconds(5);
        playerController.enabled = true;
    }

    IEnumerator DamageSprite()
    {
        sp = GetComponentInChildren<SpriteRenderer>();
        sp.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sp.color = Color.white;
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
