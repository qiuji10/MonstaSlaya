using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed;

    //SpriteRenderer sp;

    public Vector2 direction = new Vector2(1, 0);
    private Vector2 velocity;

    private void Awake()
    {
        //sp = GetComponent<SpriteRenderer>();
        Destroy(gameObject, 8);
    }

    void Update()
    {
        velocity = direction * speed;
        //if (!sp.isVisible)
        //    Destroy(gameObject);
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
        if (col.gameObject.CompareTag("Player"))
        {
            if (!col.isTrigger)
            {
                col.gameObject.GetComponent<PlayerCore>().PlayerDamaged(2);
                Destroy(gameObject);
            }
        }
    }
}