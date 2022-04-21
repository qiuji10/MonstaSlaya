using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomManager : MonoBehaviour
{
    public GameObject boss, golemStats;
    private DoorTrigger dt;

    private void Awake()
    {
        boss = FindObjectOfType<Boss_FSM>().gameObject;
        dt = GetComponentInChildren<DoorTrigger>();
        golemStats = GameObject.Find("GolemStats");
        boss.SetActive(false);
        golemStats.SetActive(false);
    }

    void Update()
    {
        if (dt.doorTriggerd)
        {
            boss.SetActive(true);
            golemStats.SetActive(true);
        }
    }
}
