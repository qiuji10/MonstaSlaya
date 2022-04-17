using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Boss_FSM : MonoBehaviour
{
    public float maxRestTime = 5f;
    public float inStateTimer;
    public float bossOriginalSpeed;
    public bool jumpTimeOut, isRage;
    private int lastNum;

    Rigidbody2D rb;
    Boss_BaseState currentState;
    EnemyBase enemy;
    Transform aimDirection;
    public CinemachineImpulseSource impSource;
    public GameObject RockPrefab;

    public Rigidbody2D Rb { get { return rb; } }
    public EnemyBase Enemy { get { return enemy; } } 
    public Boss_BaseState CurrentState { get { return currentState; } }

    public readonly Boss_REST restState = new Boss_REST();
    public readonly Boss_TRACE traceState = new Boss_TRACE();
    public readonly Boss_JUMP jumpState = new Boss_JUMP();
    public readonly Boss_RUSH rushState = new Boss_RUSH();
    public readonly Boss_SMASH smashState = new Boss_SMASH();
    public readonly Boss_THROW throwState = new Boss_THROW();
    public readonly Boss_RAGE rageState = new Boss_RAGE();

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<EnemyBase>();
        bossOriginalSpeed = Enemy.speed;
        impSource = FindObjectOfType<CinemachineImpulseSource>();
        aimDirection = transform.Find("Aimer").transform;
    }

    void Start()
    {
        SetState(restState);
    }

    void Update()
    {
        if (enemy.currenthealth < enemy.maxHealth/2 && !isRage)
        {
            SetState(rageState);
        }
        currentState.Update(this);
    }

    public void SetState(Boss_BaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public void BossRandomState()
    {
        int randNum = Random.Range(0, 6);

        while (randNum == lastNum)
        {
            randNum = Random.Range(0, 6);
        }

        switch (randNum)
        {
            case 0:
                SetState(restState);
                break;
            case 1:
                SetState(traceState);
                break;
            case 2:
                if (isRage)
                    SetState(jumpState);
                else
                    SetState(smashState);
                break;
            case 3:
                SetState(rushState);
                break;
            case 4:
                SetState(smashState);
                break;
            case 5:
                if (isRage)
                    SetState(throwState);
                else
                    SetState(rushState);
                break;
        }


        lastNum = randNum;
    }

    public void ThrowRock()
    {
        Vector3 targetDirection = (enemy.target.position - aimDirection.position).normalized;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        aimDirection.eulerAngles = new Vector3(0, 0, angle);
        GameObject bullet = Instantiate(RockPrefab, aimDirection.position, aimDirection.rotation * Quaternion.Euler(0, 0, 90));
        bullet.GetComponent<EnemyBullet>().direction = targetDirection;
    }

    public void Shake(float time)
    {
        impSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = time;
        impSource.m_DefaultVelocity.x = impSource.m_DefaultVelocity.y = -0.5f;
        impSource.GenerateImpulse();
        impSource.m_DefaultVelocity.x = impSource.m_DefaultVelocity.y = -0.1f;
        impSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = 0.2f;
    }
}
