using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : NetworkBehaviour
{
    [Header("Knight")]
    float hitTime = 0.5f;
    public int combo = 0;
    public bool knightComboTimer, isKAttack;
    public float comboTimer;
    [Space(20)]

    [Header("Archer")]
    public bool isAiming;
    private int archerDamage;
    [SerializeField] float maxAngle = 1, minAngle = -1;
    

    Rigidbody2D rb;
    PlayerCore playerCore;
    Transform aim;
    UIManager uiManager;

    [Space(20)]
    public Vector2 movement;
    [SyncVar] public Vector3 mousePos;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCore = GetComponent<PlayerCore>();
        aim = transform.Find("Aim");
        uiManager = FindObjectOfType<UIManager>();
    }


    void Update()
    {
        if (!base.IsOwner)
            return;

        if (!playerCore.isParalyzed)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if (Input.mouseScrollDelta.y > 0 || Input.mouseScrollDelta.y < 0 || Input.GetKeyDown(KeyCode.E))
            {
                playerCore.SwitchCharacter();
            }

            AimDirection();

            Attack();

            Skill();
        }
    }

    void FixedUpdate()
    {
        if (!playerCore.assassinShowtime)
            rb.MovePosition(rb.position + movement.normalized * playerCore.speed * Time.fixedDeltaTime);
    }

    public void AimDirection()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 aimDir = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        aim.eulerAngles = new Vector3(0, 0, angle);
    }

    public void Attack()
    {
        if (playerCore.playerState == PlayerCore.Character.KNIGHT)
        {
            if (playerCore.knightAtkCD < playerCore.knightAtkRate)
                playerCore.knightAtkCD += Time.deltaTime;

            else if (playerCore.knightAtkCD >= playerCore.knightAtkRate && Input.GetMouseButtonDown(0))
            {
                if (combo == 0)
                {
                    combo = 1;
                    playerCore.knightAtkCD = 0;
                    knightComboTimer = true;
                    isKAttack = true;
                }
                else if (combo == 1 && comboTimer < (playerCore.knightAtkCD + hitTime))
                {
                    combo = 2;
                    playerCore.knightAtkCD = 0;
                    comboTimer = 0;
                    isKAttack = true;
                }
                else if (combo == 2 && comboTimer < (playerCore.knightAtkCD + hitTime))
                {
                    combo = 3;
                    playerCore.knightAtkCD = 0;
                    knightComboTimer = false;
                    isKAttack = true;
                }
                else
                {
                    comboTimer = 0;
                    combo = 0;
                    playerCore.knightAtkCD = 0;
                    isKAttack = true;
                }

                if (isKAttack)
                {
                    if (combo == 0)
                        combo = 1;
                    playerCore.KnightAttack(combo);
                    if (combo == 3)
                        combo = 0;
                    isKAttack = false;
                }
            }

            if (knightComboTimer)
                comboTimer += Time.deltaTime;
            else
                comboTimer = 0;
        }
        else if (playerCore.playerState == PlayerCore.Character.ARCHER)
        {
            if (playerCore.archerAtkCD < playerCore.archerAtkRate)
                playerCore.archerAtkCD += Time.deltaTime;

            else if (playerCore.archerAtkCD >= playerCore.archerAtkRate && Input.GetMouseButton(0) && !isAiming)
            {
                AudioManager.instance.PlaySFX(playerCore.ArcherAudio, "Archer_Attack_Bow");
                playerCore.archerAtkCD = 0;
                playerCore.ArcherAttack();
                isAiming = true;
            }

            if (isAiming)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    AudioManager.instance.PlaySFX(playerCore.ArcherAudio, "Archer_Attack_Shoot");
                    int max = Mathf.CeilToInt(maxAngle * 10);
                    int min = Mathf.FloorToInt(minAngle * 10);

                    if (maxAngle <= 0 && minAngle >= 0)
                        min = max = 0;
                    
                    if (maxAngle <= 0.2f && minAngle >= -0.2f)
                        archerDamage = 8;
                    else
                        archerDamage = 4;

                    playerCore.ArcherShoot(mousePos, max, min, archerDamage);
                    maxAngle = 1;
                    minAngle = -1;
                    isAiming = false;
                }
                else if (maxAngle > 0 && minAngle < 0)
                {
                    maxAngle -= Time.deltaTime;
                    minAngle += Time.deltaTime;
                }
            }
        }
        else if (playerCore.playerState == PlayerCore.Character.ASSASSIN)
        {
            if (Input.GetMouseButtonDown(0))
            {
                playerCore.AssassinAttack();
            }
        }
    }

    public void Skill()
    {
        if (playerCore.playerState == PlayerCore.Character.ARCHER)
        {
            if (Input.GetMouseButtonDown(1) && !playerCore.archerSkill)
            {
                if (!playerCore.archerSkillCAM.activeInHierarchy)
                    playerCore.archerSkillCAM.SetActive(true);
            }
            else if (Input.GetMouseButtonUp(1) && !playerCore.archerSkill)
            {
                AudioManager.instance.PlaySFX(playerCore.ArcherAudio, "Archer_Skill_Shoot");
                AudioManager.instance.PlaySFX(playerCore.ArcherAudio, "Archer_Skill_Drop");
                uiManager.ArcherCD.fillAmount = 0;
                uiManager.ArcherText.gameObject.SetActive(true);
                playerCore.archerSkill = true;
                GameObject AOE = Instantiate(playerCore.archerAOE, new Vector3(playerCore.archerSkillCAM.transform.position.x, playerCore.archerSkillCAM.transform.position.y, -1), Quaternion.identity);
                Destroy(AOE, 2);
                playerCore.archerSkillCAM.SetActive(false);
                playerCore.archerSkillCAM.transform.position = transform.position;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (playerCore.playerState == PlayerCore.Character.KNIGHT && !playerCore.knightSkill)
            {
                AudioManager.instance.PlaySFX(playerCore.KnightAudio, "Knight_Skill");
                playerCore.knightSkill = true;
                playerCore.immunity = true;
                uiManager.KnightCD.fillAmount = 0;
                uiManager.KnightText.gameObject.SetActive(true);
                playerCore.shield.SetActive(true);
            }
            else if (playerCore.playerState == PlayerCore.Character.ASSASSIN && !playerCore.assassinSkill)
            {
                AudioManager.instance.PlaySFX(playerCore.AssassinAudio, "Assassin_Skill");
                playerCore.immunity = true;
                playerCore.trail.SetActive(true);
                uiManager.AssassinCD.fillAmount = 0;
                uiManager.AssassinText.gameObject.SetActive(true);
                playerCore.assassinShowtime = true;
                playerCore.assassinSkill = true;
                if (rb != null)
                {
                    Vector2 diff = mousePos - transform.position;
                    diff = diff.normalized * playerCore.assassinGoForce;
                    rb.AddForce(diff, ForceMode2D.Impulse);
                    StartCoroutine(AssasinGo());
                }
            }
        }  
    }

    IEnumerator AssasinGo()
    {
        yield return new WaitForSeconds(0.4f);
        rb.velocity = Vector2.zero;
        playerCore.immunity = false;
        playerCore.trail.SetActive(false);
        playerCore.assassinShowtime = false;
    }
}
