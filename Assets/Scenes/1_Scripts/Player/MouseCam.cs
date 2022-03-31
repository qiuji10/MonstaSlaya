using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCam : MonoBehaviour
{
    Transform player;

    void Awake()
    {
        player = FindObjectOfType<PlayerCore>().transform;
    }

    void Update()
    {
        Vector3 mousePos = new Vector3();
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = (player.position + mousePos) / 10;
    }
}
