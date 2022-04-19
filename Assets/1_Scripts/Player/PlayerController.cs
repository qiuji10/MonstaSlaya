using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
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

    [Space(20)]
    public Vector2 movement;
    public Vector3 mousePos;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCore = GetComponent<PlayerCore>();
        aim = transform.Find("Aim");
    }

    void Update()
    {
        if (!playerCore.isParalyzed)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if (Input.mouseScrollDelta.y > 0 || Input.mouseScrollDelta.y < 0 || Input.GetKeyDown(KeyCode.E))
            {
                playerCore.SwitchCharacter();
            }

            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 aimDir = (mousePos - transform.position).normalized;
            float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
            aim.eulerAngles = new Vector3(0, 0, angle);

            Attack();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * playerCore.speed * Time.fixedDeltaTime);
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
                    //if (combo == 3)
                    //    combo = 0;
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
                playerCore.archerAtkCD = 0;
                playerCore.ArcherAttack();
                isAiming = true;
            }

            if (isAiming)
            {
                if (Input.GetMouseButtonUp(0))
                {
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
}
