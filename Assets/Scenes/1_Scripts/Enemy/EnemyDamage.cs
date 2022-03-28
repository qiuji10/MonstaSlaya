using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    Enemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (!col.isTrigger && Vector2.Distance(transform.position, enemy.target.position) < 1.5f)
            {
                col.gameObject.GetComponent<PlayerCore>().PlayerDamaged(enemy.damage);
            }
        }
    }
}
