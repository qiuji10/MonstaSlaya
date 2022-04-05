using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_JUMP : Boss_BaseState
{
    public override void EnterState(Boss_FSM boss)
    {
        Debug.Log("Jump state");
        boss.Enemy.speed = 0;
        boss.Enemy.Anim.SetTrigger("JumpStart");
    }

    public override void Update(Boss_FSM boss)
    {
        boss.inStateTimer += Time.deltaTime;
        if (boss.inStateTimer >= 0.6f && boss.Enemy.speed == 0)
        {
            boss.Enemy.speed = boss.bossOriginalSpeed;
            boss.inStateTimer = 0;
        }

        if (boss.Enemy.speed != 0)
        {
            boss.Enemy.Anim.ResetTrigger("JumpStart");
            boss.Enemy.Anim.SetTrigger("JumpTargeting");

            if (boss.inStateTimer >= 3)
            {
                boss.jumpTimeOut = true;
                boss.inStateTimer = 0;
            }

            Vector3 targetPos = boss.Enemy.target.position;

            if (Vector2.Distance(boss.Enemy.transform.position, targetPos) > boss.Enemy.minDistance)
            {
                boss.Rb.MovePosition(Vector2.MoveTowards(boss.transform.position, targetPos, boss.Enemy.speed * Time.deltaTime));
            }
            if (Vector2.Distance(boss.Enemy.transform.position, targetPos) <= boss.Enemy.minDistance || boss.jumpTimeOut)
            {
                boss.jumpTimeOut = false;
                boss.inStateTimer = 0;
                boss.Enemy.Anim.ResetTrigger("JumpTargeting");
                boss.Enemy.Anim.SetTrigger("JumpEnd");
                boss.StartCoroutine(boss.Shake());
                boss.SetState(boss.traceState);
            }
        }
    }
}
