using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBounce : MonoBehaviour
{
    public float speed;
    bool bounced;
    Rigidbody2D rb;
    public Vector2 direction;
    Vector3 lastVelocity;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 8);
    }

    void Update()
    {
        if (!bounced)
            rb.AddForce(direction * speed);
        lastVelocity = rb.velocity;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Wall"))
        {
            if (!bounced)
            {
                float speed = lastVelocity.magnitude;
                Vector3 direction = Vector3.Reflect(lastVelocity.normalized, col.GetContact(0).normal);
                rb.velocity = direction * speed;
                bounced = true;
            }
            else
                Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (!col.isTrigger)
            {
                col.gameObject.GetComponent<PlayerCore>().PlayerDamaged(4);
                Debug.Log("DAMAGE");
                Destroy(gameObject);
            }
        }
    }
}
