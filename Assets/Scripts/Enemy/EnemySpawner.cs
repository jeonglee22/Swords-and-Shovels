using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<Transform> spawnPoints = new List<Transform>();
    private List<bool> checkUnit = new List<bool>();

    public List<EnemyDatas> datas = new List<EnemyDatas>();
    public Enemy prefab;

    private void Start()
    {
        for(int i = 0; i < spawnPoints.Count; i++)
        {
            CreateEnemy(spawnPoints[i]);
            checkUnit.Add(true);
        }
    }

    private void Update()
    {
        CheckEnemy();
    }

    public void CreateEnemy(Transform point)
    {
        var enemy = Instantiate(prefab, point.position, point.rotation);
        enemy.Setup(datas[Random.Range(0, datas.Count)]);
    }

    private void CheckEnemy()
    {
        for(int i = 0; i < spawnPoints.Count; i++)
        {
            if (checkUnit[i])
            {
                continue;
            }
            else
            {
                CreateEnemy(spawnPoints[i]);
            }
        }
    }
}
