using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    public GameObject C1, C2, C3;
    Rigidbody2D rb;

    Vector2 movement;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.I))
        {
            SwitchCharacter();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);
    }

    public void SwitchCharacter()
    {
        if (C1.activeInHierarchy)
        {
            C1.SetActive(false);
            C2.SetActive(true);
        }
        else if (C2.activeInHierarchy)
        {
            C2.SetActive(false);
            C3.SetActive(true);
        }
        else if (C3.activeInHierarchy)
        {
            C3.SetActive(false);
            C1.SetActive(true);
        }
    }
}
