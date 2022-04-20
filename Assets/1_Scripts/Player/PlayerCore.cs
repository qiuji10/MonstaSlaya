using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCore : MonoBehaviour
{
    public enum Character { KNIGHT, ARCHER, ASSASSIN };

    Animator animator;
    PlayerController playerController;
    SpriteRenderer sp;
    CinemachineImpulseSource impSource;
    GameObject paralyzedSymbol;
    UIManager uiManager;
    GameSceneManager gsm;
    Timer timer;
    [SerializeField] AudioData knightAudio, archerAudio, assassinAudio;
    [SerializeField] GameStats gameStats;

    public AudioData KnightAudio { get { return knightAudio; } }
    public AudioData ArcherAudio { get { return archerAudio; } }
    public AudioData AssassinAudio { get { return assassinAudio; } }

    [Header("Player")]
    public int maxHealth;
    public int currentHealth;
    public int maxShield;
    public int currentShield;
    public float speed = 10f, recoverTimePerSec = 3f, recoverTimer;
    public bool isParalyzed, statusChanged, immunity;
    private float paralyzedTimer;
    [Space(20)]

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
        timer = FindObjectOfType<Timer>();
        gsm = FindObjectOfType<GameSceneManager>();
        impSource = FindObjectOfType<CinemachineImpulseSource>();
        uiManager = FindObjectOfType<UIManager>();
        paralyzedSymbol = transform.Find("Paralyzed").gameObject;
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
        {
            AudioManager.instance.PlaySFX(knightAudio, "Knight_Attack_1");
            animator.SetTrigger("KnightAttack1");
        }
        else if (atkCombo == 2)
        {
            AudioManager.instance.PlaySFX(knightAudio, "Knight_Attack_2");
            animator.SetTrigger("KnightAttack2");
        }
        else if (atkCombo == 3)
        {
            AudioManager.instance.PlaySFX(knightAudio, "Knight_Attack_3");
            animator.SetTrigger("KnightAttack3");
        }
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
        AudioManager.instance.PlaySFX(AssassinAudio, "Assassin_Attack");
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
                enemy.GetComponent<EnemyBase>().TakeDamage(damage, transform.position);
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
            if (currentShield <= 0)
            {
                currentHealth -= damage;
                if (currentHealth < 0 || currentHealth > maxHealth)
                {
                    currentHealth = 0;
                }
            }

            if (currentShield > 0)
            {
                currentShield -= damage;
                recoverTimer = currentShield;

                if (currentShield < 0 && currentHealth > 0)
                {
                    int overflowDamage = -(currentShield);
                    currentShield = 0;
                    uiManager.CurrentShieldText.text = currentShield.ToString();
                    currentHealth -= overflowDamage;
                    if (currentHealth < 0)
                    {
                        currentHealth = 0;
                    }
                    uiManager.CurrentHealthText.text = currentHealth.ToString();
                }
            }

            statusChanged = true;
            uiManager.CurrentShieldText.text = currentShield.ToString();
            uiManager.CurrentHealthText.text = currentHealth.ToString();

            StartCoroutine(DamageSprite());

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                playerController.enabled = false;

                if (playerState == Character.KNIGHT)
                {
                    gameStats.state = Character.KNIGHT;
                    animator.SetTrigger("KnightDeath");
                }
                else if (playerState == Character.ARCHER)
                {
                    gameStats.state = Character.ARCHER;
                    animator.SetTrigger("ArcherDeath");
                }
                else if (playerState == Character.ASSASSIN)
                {
                    gameStats.state = Character.ASSASSIN;
                    animator.SetTrigger("AssassinDeath");
                }

                gameStats.time = timer.timerText.text;
                gsm.SwitchScene(2);
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
