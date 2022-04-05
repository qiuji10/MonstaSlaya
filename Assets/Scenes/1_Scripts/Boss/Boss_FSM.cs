using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Boss_FSM : MonoBehaviour
{
    public float maxRestTime = 5f;
    public float inStateTimer;
    public float bossOriginalSpeed;
    public bool jumpTimeOut;

    Rigidbody2D rb;
    Boss_BaseState currentState;
    EnemyBase enemy;
    public CinemachineImpulseSource impSource;

    public Rigidbody2D Rb { get { return rb; } }
    public EnemyBase Enemy { get { return enemy; } }
    public Boss_BaseState CurrentState { get { return currentState; } }

    public readonly Boss_REST restState = new Boss_REST();
    public readonly Boss_TRACE traceState = new Boss_TRACE();
    public readonly Boss_JUMP jumpState = new Boss_JUMP();
    public readonly Boss_RUSH rushState = new Boss_RUSH();
    public readonly Boss_SMASH smashState = new Boss_SMASH();

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<EnemyBase>();
        bossOriginalSpeed = Enemy.speed;
        impSource = FindObjectOfType<CinemachineImpulseSource>();
    }

    void Start()
    {
        SetState(rushState);
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

    public IEnumerator Shake()
    {
        impSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = 1;
        impSource.GenerateImpulse();
        yield return new WaitForSeconds(1);
        impSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = 0.2f;
    }
}
