using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    [SerializeField] int radius;

    PlayerController playerController;

    void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            Vector3 dir = (playerController.mousePos - playerController.transform.position).normalized;
            Vector3 cursorVector = dir * radius;
            transform.position = playerController.transform.position + cursorVector;
        }
    }
}
