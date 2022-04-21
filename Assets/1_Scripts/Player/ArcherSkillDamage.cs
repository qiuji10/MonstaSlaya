using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSkillDamage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            col.gameObject.GetComponentInParent<EnemyBase>().TakeDamage(10);

            if (col.gameObject.GetComponent<Boss_FSM>() != null)
            {
                col.gameObject.GetComponent<Boss_FSM>().healthBar.BossHealthChange(10);
            }
        }
    }
}
