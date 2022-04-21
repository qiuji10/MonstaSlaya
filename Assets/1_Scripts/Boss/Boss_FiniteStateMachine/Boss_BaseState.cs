using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss_BaseState
{
    public abstract void EnterState(Boss_FSM boss);

    public abstract void Update(Boss_FSM boss);
}