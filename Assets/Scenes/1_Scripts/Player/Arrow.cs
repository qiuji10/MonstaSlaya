using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed;

    SpriteRenderer sp;

    public Vector2 direction = new Vector2(1, 0);
    private Vector2 velocity;

    private void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
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
            Debug.Log("Hitting enemy");
            col.gameObject.GetComponentInParent<Enemy>().TakeDamage(2);
            Destroy(gameObject);
        }
    }
}