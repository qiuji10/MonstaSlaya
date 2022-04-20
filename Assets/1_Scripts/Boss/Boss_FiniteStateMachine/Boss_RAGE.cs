using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_RAGE : Boss_BaseState
{
    public override void EnterState(Boss_FSM boss)
    {
        boss.Enemy.speed = 0;
        boss.Enemy.Anim.SetTrigger("RageTrigger");
        AudioManager.instance.PlaySFX(boss.GolemAudio, "Rage");
        boss.isRage = true;
    }

    public override void Update(Boss_FSM boss)
    {
        boss.inStateTimer += Time.deltaTime;

        if (boss.inStateTimer > 4)
        {
            boss.Enemy.speed = boss.bossOriginalSpeed;
            boss.inStateTimer = 0;
            boss.SetState(boss.restState);
        }
    }
}
