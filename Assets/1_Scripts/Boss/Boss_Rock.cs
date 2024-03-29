using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Rock : MonoBehaviour
{
    public float speed;
    int damage = 2;

    public Vector2 direction = new Vector2(1, 0);
    private Vector2 velocity;

    private void Awake()
    {
        Destroy(gameObject, 8);
    }

    void Update()
    {
        velocity = direction * speed;
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
            col.gameObject.GetComponent<PlayerCore>().PlayerDamaged(damage);
            col.gameObject.GetComponent<PlayerCore>().isParalyzed = true;
            Destroy(gameObject);
        }
    }
}
