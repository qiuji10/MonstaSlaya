using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] List<GameObject> walls = new List<GameObject>();
    [SerializeField] AudioData battleBGM;
    public bool doorTriggerd;

    public void SetWallStatus(bool status)
    {
        foreach (GameObject wall in walls)
        {
            wall.SetActive(status);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (!doorTriggerd)
            {
                if (GetComponentInParent<BossRoomManager>() != null)
                    AudioManager.instance.PlayBGM(battleBGM, "Boss");
                else
                    AudioManager.instance.PlayBGM(battleBGM, "NormalBattle");
                SetWallStatus(true);
            }
            doorTriggerd = true;
        }
    }
}
