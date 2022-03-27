using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosestEnemy : MonoBehaviour
{
    public void FindClosestEnemy(ref Vector3 enemyPosition)
    {
        float distanceToClosestEnemy = Mathf.Infinity;
        Enemy1 closestEnemy = null;

        Enemy1[] allEnemies = FindObjectsOfType<Enemy1>();

        foreach (Enemy1 currentEnemy in allEnemies)
        {
            float distanceToEnemy = (currentEnemy.transform.position - transform.position).sqrMagnitude;
            if (distanceToEnemy < distanceToClosestEnemy)
            {
                distanceToClosestEnemy = distanceToEnemy;
                closestEnemy = currentEnemy;
                enemyPosition = closestEnemy.transform.position;
            }
        }
        Debug.DrawLine(transform.position, closestEnemy.transform.position);
    }
}
