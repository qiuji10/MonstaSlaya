using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class EnemyData : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int maxHealth;
    public int currenthealth;
    public int damage;
    public float speed;

    public GameObject damageText;
    SpriteRenderer sp;

    public void TakeDamage(int damaged)
    {
        Debug.Log("Enemy Hitted");
        GameObject DamageText = Instantiate(damageText, new Vector3(transform.position.x, transform.position.y, -1), Quaternion.identity);
        DamageText.GetComponent<TextMeshPro>().text = damaged.ToString();
        Destroy(DamageText, 0.3f);
        currenthealth -= damaged;
        StartCoroutine(DamageSprite());

        if (currenthealth <= 0)
        {
            Debug.Log("EnemyDie");

            if (gameObject != null)
                Destroy(gameObject);
            else
                Debug.LogWarning("Game object couldn't found! Error");
        }
    }

    IEnumerator DamageSprite()
    {
        sp = GetComponentInChildren<SpriteRenderer>();
        sp.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sp.color = Color.white;
    }
}