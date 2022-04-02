using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    EnemyBase enemy;
    CircleCollider2D collider2d;

    private void Awake()
    {
        enemy = GetComponentInParent<EnemyBase>();
        collider2d = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (!col.isTrigger && Vector2.Distance(transform.position, enemy.target.position) < collider2d.radius)
            {
                Debug.Log("Hitted by" + gameObject.transform.parent.name);
                col.gameObject.GetComponent<PlayerCore>().PlayerDamaged(enemy.damage);
            }
        }
    }
}
