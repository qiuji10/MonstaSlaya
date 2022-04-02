using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_REST_State : Boss_BaseState
{
    public override void EnterState(Boss_FSM boss)
    {
        
    }

    public override void Update(Boss_FSM boss)
    {
        boss.Enemy.WalkAnimation();
        float time = boss.maxRestTime;
        boss.Enemy.characterVelocity = 20;
        boss.Enemy.RestMovement();

        time -= Time.deltaTime;

        if (time <= 0)
        {
            boss.maxRestTime = 5f;
            boss.SetState(boss.traceState);
        }
    }
}
