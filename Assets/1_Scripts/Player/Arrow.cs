using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Arrow : MonoBehaviour
{
    public float speed;
    public int dmg = 4;

    SpriteRenderer sp;
    CinemachineImpulseSource impSource;

    public Vector2 direction = new Vector2(1, 0);
    private Vector2 velocity;

    private void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
        impSource = FindObjectOfType<CinemachineImpulseSource>();
    }

    void Update()
    {
        velocity = direction * speed;
        if (!sp.isVisible)
            Destroy(gameObject);
            //gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        pos += velocity * Time.deltaTime;
        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            impSource.GenerateImpulse();
            col.gameObject.GetComponentInParent<EnemyBase>().TakeDamage(dmg, transform.position);
            Destroy(gameObject);

            if (col.gameObject.GetComponent<Boss_FSM>() != null)
            {
                col.gameObject.GetComponent<Boss_FSM>().healthBar.BossHealthChange(dmg);
            }
        }

        if (col.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}