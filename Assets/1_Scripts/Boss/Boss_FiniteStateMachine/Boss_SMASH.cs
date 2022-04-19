using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_SMASH : Boss_BaseState
{
    public override void EnterState(Boss_FSM boss)
    {
        boss.Enemy.Anim.ResetTrigger("Smash");
        boss.Enemy.Anim.SetTrigger("Smash");

        if (boss.isRage)
            boss.BulletCircle(100);
        else
            boss.BulletCircle(4);
    }

    public override void Update(Boss_FSM boss)
    {
        boss.inStateTimer += Time.deltaTime;


        if (boss.inStateTimer >= 1)
        {
            boss.inStateTimer = 0;
            if (boss.smashNum > 0)
            {
                boss.smashNum--;
                boss.SetState(boss.smashState);
            }
            else
            {
                boss.smashNum = 3;
                boss.SetState(boss.restState);
            }
            
        }
    }
}
