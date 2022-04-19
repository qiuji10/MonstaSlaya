using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class Boss_FSM : MonoBehaviour
{
    public float maxRestTime = 5f;
    public float inStateTimer;
    public float bossOriginalSpeed;
    public bool jumpTimeOut, isRage;
    public int smashNum = 3;
    private int lastNum;

    Rigidbody2D rb;
    Boss_BaseState currentState;
    EnemyBase enemy;
    Transform aimDirection;
    Transform aimDirection2;
    public Boss_HealthSlider healthBar;
    public CinemachineImpulseSource impSource;
    public GameObject RockPrefab, enemyBullet;

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
        aimDirection2 = transform.Find("Aimer2").transform;
        healthBar.GetComponent<Slider>().maxValue = enemy.maxHealth;
    }

    void Start()
    {
        SetState(smashState);
    }

    void Update()
    {
        if (enemy.currenthealth < enemy.maxHealth/2 && !isRage)
        {
            Enemy.speed = bossOriginalSpeed = (bossOriginalSpeed * 1.5f);
            enemy.damage += 2;
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
        int randNum = Random.Range(0, 5);

        while (randNum == lastNum)
        {
            randNum = Random.Range(0, 5);
        }

        switch (randNum)
        {
            case 0:
                SetState(traceState);
                break;
            case 1:
                SetState(smashState);
                break;
            case 2:
                SetState(rushState);
                break;
            case 3:
                SetState(jumpState);
                break;
            case 4:
                SetState(throwState);
                break;
        }
        lastNum = randNum;
    }

    public void ThrowRock()
    {
        Vector3 targetDirection = (enemy.target.position - aimDirection.position).normalized;
        if (!isRage)
        {
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            aimDirection.eulerAngles = new Vector3(0, 0, angle);
            GameObject bullet = Instantiate(RockPrefab, aimDirection.position, aimDirection.rotation * Quaternion.Euler(0, 0, 90));
            bullet.GetComponent<Boss_Rock>().direction = targetDirection;
        }
        else
        {
            targetDirection = Quaternion.Euler(0, 0, 50) * targetDirection;
            for (int i = 0; i < 3; i++)
            {
                GameObject bullet = Instantiate(RockPrefab, aimDirection.position, Quaternion.identity);
                targetDirection = Quaternion.Euler(0, 0, -20) * targetDirection;
                bullet.GetComponent<Boss_Rock>().direction = targetDirection;
            }
        }
    }

    public void BulletCircle(int bulletNum)
    {
        Vector3 targetDirection = (enemy.target.position - aimDirection2.position).normalized;
        targetDirection = Quaternion.Euler(0, 0, 50) * targetDirection;
        for (int i = 0; i < bulletNum; i++)
        {
            GameObject bullet = Instantiate(enemyBullet, aimDirection2.position, Quaternion.identity);
            targetDirection = Quaternion.Euler(0, 0, -20) * targetDirection;
            bullet.GetComponent<EnemyBulletBounce>().direction = targetDirection;
        }
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
