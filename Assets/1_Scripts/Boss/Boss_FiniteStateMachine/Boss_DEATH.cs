using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_DEATH : Boss_BaseState
{
    public override void EnterState(Boss_FSM boss)
    {
        boss.Enemy.Anim.SetTrigger("Death");
    }

    public override void Update(Boss_FSM boss)
    {

    }
}
