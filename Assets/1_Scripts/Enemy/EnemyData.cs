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
    private float knockForce = 10;

    public GameObject damageText;
    SpriteRenderer sp;

    public void TakeDamage(int damaged, Vector3 playerPos)
    {
        Debug.Log("Enemy Hitted");
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        if (rigidbody != null)
        {
            Debug.Log("Rigidbody");
            Vector2 diff = transform.position - playerPos;
            diff = diff.normalized * knockForce;
            rigidbody.AddForce(diff, ForceMode2D.Impulse);
            StartCoroutine(Knockback(rigidbody));
        }
        GameObject DamageText = Instantiate(damageText, new Vector3(transform.position.x, transform.position.y, -1), Quaternion.identity);
        DamageText.GetComponent<TextMeshPro>().text = damaged.ToString();
        Destroy(DamageText, 0.3f);
        currenthealth -= damaged;
        StartCoroutine(DamageSprite());

        if (currenthealth <= 0)
        {
            if (gameObject != null)
                Destroy(gameObject);
            else
                Debug.LogWarning("Couldn't found enemy gameobject!");
        }
    }

    IEnumerator DamageSprite()
    {
        sp = GetComponentInChildren<SpriteRenderer>();
        sp.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sp.color = Color.white;
    }

    IEnumerator Knockback(Rigidbody2D rb)
    {
        yield return new WaitForSeconds(0.2f);
        rb.velocity = Vector2.zero;
    }
}