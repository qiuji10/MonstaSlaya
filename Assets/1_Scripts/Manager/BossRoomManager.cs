using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomManager : MonoBehaviour
{
    public GameObject boss;
    private DoorTrigger dt;

    private void Awake()
    {
        boss = FindObjectOfType<Boss_FSM>().gameObject;
        dt = GetComponentInChildren<DoorTrigger>();
        boss.SetActive(false);
    }

    void Update()
    {
        if (dt.doorTriggerd)
        {
            boss.SetActive(true);
        }
    }
}
