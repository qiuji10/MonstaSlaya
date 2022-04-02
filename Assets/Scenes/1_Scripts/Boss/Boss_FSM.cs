using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_FSM : MonoBehaviour
{
    public float maxRestTime = 5f;

    Rigidbody2D rb;
    Boss_BaseState currentState;
    EnemyBase enemy;

    public Rigidbody2D Rb { get { return rb; } }
    public EnemyBase Enemy { get { return enemy; } }
    public Boss_BaseState CurrentState { get { return currentState; } }

    public readonly Boss_REST_State restState = new Boss_REST_State();
    public readonly Boss_TRACE_State traceState = new Boss_TRACE_State();

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<EnemyBase>();
    }

    void Start()
    {
        SetState(restState);
    }

    void Update()
    {
        currentState.Update(this);
    }

    public void SetState(Boss_BaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
}
