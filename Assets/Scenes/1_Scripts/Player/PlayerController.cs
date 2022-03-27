using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    PlayerCore playerCore;

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

        //if (Time.time >= playerCore.knightNxtAtk && playerCore.playerState == PlayerCore.Character.KNIGHT)
        //{
        //    if (Input.GetKeyDown(KeyCode.J))
        //    {
        //        playerCore.KnightAttack();
        //        playerCore.knightNxtAtk = Time.time + 1f / playerCore.knightAtkRate;
        //    }
        //}

        if (playerCore.playerState == PlayerCore.Character.KNIGHT)
        {
            if (playerCore.knightAtkCD < playerCore.knightAtkRate)
                playerCore.knightAtkCD += Time.deltaTime;

            else if (playerCore.knightAtkCD >= playerCore.knightAtkRate && Input.GetKeyDown(KeyCode.J))
            {
                playerCore.knightAtkCD = 0;
                playerCore.KnightAttack();
            }
        }
        else if (playerCore.playerState == PlayerCore.Character.ARCHER)
        {
            if (playerCore.archerAtkCD < playerCore.archerAtkRate)
                playerCore.archerAtkCD += Time.deltaTime;

            else if (playerCore.archerAtkCD >= playerCore.archerAtkRate && Input.GetKeyDown(KeyCode.J))
            {
                playerCore.archerAtkCD = 0;
                playerCore.ArcherAttack();
            }
        }
        else if (playerCore.playerState == PlayerCore.Character.ASSASSIN)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                playerCore.AssassinAttack();
            }
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * playerCore.speed * Time.fixedDeltaTime);
    }
}
