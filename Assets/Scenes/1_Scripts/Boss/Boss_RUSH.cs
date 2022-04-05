using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_RUSH : Boss_BaseState
{
    public override void EnterState(Boss_FSM boss)
    {
        Debug.Log("Rush state");
        boss.Enemy.speed = 0;
        boss.Enemy.Anim.SetTrigger("RushStart");
        boss.Enemy.Anim.SetTrigger("RushRoll");
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
            if (Vector2.Distance(boss.Enemy.transform.position, boss.Enemy.target.position) > boss.Enemy.minDistance)
            {
                boss.Enemy.transform.position = Vector2.MoveTowards(boss.Enemy.transform.position, boss.Enemy.target.position, boss.Enemy.speed * 2 * Time.deltaTime);
            }
            else
            {
                boss.Enemy.Anim.ResetTrigger("RushRoll");
                boss.Enemy.Anim.SetTrigger("RushEnd");
                boss.SetState(boss.smashState);
            }
        }
    }
}
