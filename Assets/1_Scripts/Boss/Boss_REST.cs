using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_REST : Boss_BaseState
{
    float originRestTime;

    public override void EnterState(Boss_FSM boss)
    {
        Debug.Log("Rest state");
        originRestTime = boss.maxRestTime;
        boss.Enemy.characterVelocity = 10;
    }

    public override void Update(Boss_FSM boss)
    {
        boss.Enemy.WalkAnimation();
        boss.Enemy.RestMovement();

        boss.maxRestTime -= Time.deltaTime;
        if (boss.maxRestTime <= 0)
        {
            boss.maxRestTime = originRestTime;
            boss.Enemy.characterVelocity = 2.5f;
            boss.BossRandomState();
        }
    }
}
