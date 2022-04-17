using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTrigger : MonoBehaviour
{
    [SerializeField] List<GameObject> walls = new List<GameObject>();

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
            SetWallStatus(true);
            GetComponentInParent<WaveSpawner>().state = WaveSpawner.SpawnState.WAITING;
        }
    }
}
