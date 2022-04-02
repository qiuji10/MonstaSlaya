using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_TRACE_State : Boss_BaseState
{
    public override void EnterState(Boss_FSM boss)
    {

    }

    public override void Update(Boss_FSM boss)
    {
        boss.Enemy.WalkAnimation();
        boss.Enemy.FacingTarget();

        if (Vector2.Distance(boss.Enemy.transform.position, boss.Enemy.target.position) > boss.Enemy.minDistance)
        {
            boss.Enemy.transform.position = Vector2.MoveTowards(boss.Enemy.transform.position, boss.Enemy.target.position, boss.Enemy.speed * 10 * Time.deltaTime);
            //rb.MovePosition(Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime));
        }
        else
        {
            boss.SetState(boss.restState);
        }
    }
}
