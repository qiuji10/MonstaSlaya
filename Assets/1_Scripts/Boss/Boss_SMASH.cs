using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_SMASH : Boss_BaseState
{
    public override void EnterState(Boss_FSM boss)
    {
        Debug.Log("Smash state");
        boss.Enemy.Anim.SetTrigger("Smash");
    }

    public override void Update(Boss_FSM boss)
    {
        boss.SetState(boss.restState);
    }
}
