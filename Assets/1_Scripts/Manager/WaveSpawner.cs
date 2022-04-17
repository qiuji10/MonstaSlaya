using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState { NOTHING, SPAWNING, WAITING, COUNTING };

    [System.Serializable]
    public class Enemies
    {
        public Transform enemy1;
        public int count1;
        public Transform enemy2;
        public int count2;
        public Transform enemy3;
        public int count3;
        public Transform enemy4;
        public int count4;
    }

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Enemies enemies;
        public float rate;
    }

    [Header("Wave Settings")]
    [SerializeField]
    private float waveCounddown;
    [SerializeField]
    public float waveIntervalTime = 5f;
    private float searchCountdown = 1f;
    private int nextWave = 0;
    private bool isEnd;
    [SerializeField]
    private Vector2 maxSpawnPos, minSpawnPos;

    [Header("Enemy Initialization")]
    public Wave[] waves;

    [Header("Game State")]
    public SpawnState state = SpawnState.NOTHING;

    private void Start()
    {
        waveCounddown = waveIntervalTime;
    }

    private void Update()
    {
        if (state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive())
            {
                if (isEnd)
                {
                    state = SpawnState.NOTHING;
                    GetComponentInChildren<SpawnerTrigger>().SetWallStatus(false);
                }
                else
                    WaveCompleted(waves[nextWave]);
            }
            else
                return;
        }

        if (waveCounddown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpwanWave(waves[nextWave]));
            }
        }
        else if (state == SpawnState.NOTHING)
            return;
        else
            waveCounddown -= Time.deltaTime;

    }

    void WaveCompleted(Wave wave)
    {
        state = SpawnState.COUNTING;
        waveCounddown = waveIntervalTime;

        //open door
    }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
                return false;
        }
        return true;
    }

    IEnumerator SpwanWave(Wave _wave)
    {
        state = SpawnState.SPAWNING;
        int one = _wave.enemies.count1, two = _wave.enemies.count2, three = _wave.enemies.count3, four = _wave.enemies.count4;
        int allcount = one + two + three + four;
        for (int i = 0; i < allcount; i++)
        {
            int r = Random.Range(1, 5);
            switch (r)
            {
                case 1:
                    if (one == 0)
                    {
                        allcount++;
                        break;
                    }
                    SpawnEnemy(_wave.enemies.enemy1);
                    yield return new WaitForSeconds(1f / _wave.rate);
                    one--;
                    continue;

                case 2:
                    if (two == 0)
                    {
                        allcount++;
                        break;
                    }
                    SpawnEnemy(_wave.enemies.enemy2);
                    yield return new WaitForSeconds(1f / _wave.rate);
                    two--;
                    continue;

                case 3:
                    if (three == 0)
                    {
                        allcount++;
                        break;
                    }
                    SpawnEnemy(_wave.enemies.enemy3);
                    yield return new WaitForSeconds(1f / _wave.rate);
                    three--;
                    continue;

                case 4:
                    if (four == 0)
                    {
                        allcount++;
                        break;
                    }
                    SpawnEnemy(_wave.enemies.enemy4);
                    yield return new WaitForSeconds(1f / _wave.rate);
                    four--;
                    continue;
            }
        }

        if (nextWave < waves.Length - 1)
            nextWave++;
        else
        {
            waveCounddown = 5;
            isEnd = true;
        }
            

        state = SpawnState.WAITING;
        yield break;
    }

    void SpawnEnemy(Transform _enemy)
    {
        Vector3 pos = new Vector3(Random.Range(minSpawnPos.x, maxSpawnPos.x), Random.Range(minSpawnPos.y,maxSpawnPos.y));
        Transform enemy = Instantiate(_enemy, pos, transform.rotation);
    }
}
