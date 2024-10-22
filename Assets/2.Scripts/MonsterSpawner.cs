using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnerPoint = null;
    [SerializeField] private int SpawnCount = 10;
    private int _currentSpawnCount = 0;

    Vector3 RandomPos => _spawnerPoint[Random.Range(0, _spawnerPoint.Count)].position;

    private void Start()
    {
        StartCoroutine(DelaySpawn());
    }

    private IEnumerator DelaySpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            Spawn();

            if (_currentSpawnCount >= SpawnCount)
                break;
        }
    }

    private void Spawn()
    {
        _currentSpawnCount++;

        MonsterMovement monster = Manager.Instance.Pool.GetObject(E_PoolType.Monster).
            GameObject.GetComponent<MonsterMovement>();

        monster.transform.position = RandomPos;

        monster.Init();
    }
}
