using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_THROW : Boss_BaseState
{
    public override void EnterState(Boss_FSM boss)
    {
        boss.Enemy.Anim.SetTrigger("ThrowStart");
        boss.Enemy.speed = 0;
    }

    public override void Update(Boss_FSM boss)
    {
        boss.inStateTimer += Time.deltaTime;
        if (boss.inStateTimer >= 0.75f && boss.Enemy.speed == 0)
        {
            boss.Enemy.Anim.ResetTrigger("ThrowStart");
            boss.Enemy.Anim.SetTrigger("ThrowEnd");
            boss.ThrowRock();
            boss.Enemy.speed = boss.bossOriginalSpeed;
            boss.inStateTimer = 0;
        }

        if (boss.Enemy.speed != 0)
        {
            boss.inStateTimer = 0;
            boss.SetState(boss.restState);
        }
    }
}
