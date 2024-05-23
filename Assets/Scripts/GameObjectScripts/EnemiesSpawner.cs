using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using static EnemiesSpawnerSystem;
using Random = Unity.Mathematics.Random;

public class EnemiesSpawner : MonoBehaviour
{
    public int Grimonk_BrownSpawnQuatity;
    public GameObject Grimonk_Brown;


    public int SlimeBlock_RedSpawnQuatity;
    public GameObject SlimeBlock_Red;


    private Random random;
    private uint updateCount;
    private int currentLevel;
    private List<float2> spawnPositionList;

    public GameObject player;

    public int enemyCounter;
    public List<GameObject> EnemyList;


    // Start is called before the first frame update

    private void Awake()
    {
        enemyCounter = 0;
    }
    void Start()
    {
        currentLevel = 1;
        EnemyList = new List<GameObject>();
        spawnPositionList = new List<float2>
        {
            new float2(-60, 35),
            new float2(-45, 35),
            new float2(-30, 35),
            new float2(-15, 35),
            new float2(0, 35),
            new float2(15, 35),
            new float2(30, 35),
            new float2(45, 35),
            new float2(60, 35),
            new float2(-60, -35),
            new float2(-45, -35),
            new float2(-30, -35),
            new float2(-15, -35),
            new float2(0, -35),
            new float2(15, -35),
            new float2(30, -35),
            new float2(45, -35),
            new float2(60, -35),
            new float2(-60, 17.5f),
            new float2(-60, 0),
            new float2(60, -17.5f),
            new float2(60, 17.5f),
            new float2(60, 0)
        };

        random = Random.CreateFromIndex(updateCount++);
    }

    // Update is called once per frame
    void Update()
    {
        enemyCounter = EnemyList.Count;
        if (enemyCounter <= 0)
        {
            currentLevel += 1;
            if (Grimonk_BrownSpawnQuatity > 0)
            {
                for(int i = 0; i< Grimonk_BrownSpawnQuatity; i++)
                {
                    EnemyList.Add(InstantiatePrefab(Grimonk_Brown));
                }               
            }

            if (SlimeBlock_RedSpawnQuatity > 0)
            {
                for (int i = 0; i < SlimeBlock_RedSpawnQuatity; i++)
                {
                    var enemy = InstantiatePrefab(SlimeBlock_Red);
                    enemy.GetComponent<EnemyGunControl>().player = player;
                    EnemyList.Add(enemy);
                }
            }
        }
    }

    public GameObject InstantiatePrefab(GameObject prefab)
    {
        var enemy = Instantiate(prefab);
        int positionSelection = (enemy.GetComponent<EnemyControl>().random.NextInt(0, 23));      
        enemy.transform.position = new float3(spawnPositionList[positionSelection].x, spawnPositionList[positionSelection].y, 0) + enemy.GetComponent<EnemyControl>().random.NextInt3(new int3(-4, -4, 0), new int3(4, 4, 0));
        enemy.transform.rotation = quaternion.identity;
        enemy.GetComponent<EnemyControl>().player = player;
        enemy.GetComponent<EnemyControl>().EnemySpawner = this.gameObject;
        enemy.GetComponent<EnemyControl>().currentHitPoint = Mathf.RoundToInt(enemy.GetComponent<EnemyControl>().maxHP + enemy.GetComponent<EnemyControl>().maxHP * (currentLevel - 1) * 0.1f);
        enemy.GetComponent<EnemyControl>().damage += Mathf.RoundToInt(currentLevel * 0.5f);
        return enemy;
    }
}
