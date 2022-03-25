using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    public enum Character { KNIGHT, ARCHER, ASSASSIN };

    public float speed = 5f;
    public float knightAtkRate = 2f;
    public float knightNxtAtk = 0f;
    public int knightDamage = 1;

    Animator animator;

    public Character playerState = Character.KNIGHT;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
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
}
