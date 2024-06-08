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
    private static EnemiesSpawner instance;
    
    public static EnemiesSpawner Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EnemiesSpawner>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(EnemiesSpawner).Name);
                    instance = singletonObject.AddComponent<EnemiesSpawner>();
                }
            }
            return instance;
        }
    }

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

    private IEnemyFactory enemyFactory;


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
                enemyFactory = new GrimonkBrownFactory(player, Grimonk_Brown, this.gameObject);
                for (int i = 0; i < Grimonk_BrownSpawnQuatity; i++)
                {
                    var enemy = enemyFactory.CreateEnemy(currentLevel, spawnPositionList);
                    EnemyList.Add(enemy);
                }
            }

            if (SlimeBlock_RedSpawnQuatity > 0)
            {
                enemyFactory = new SlimeBlockRedFactory(player, SlimeBlock_Red, this.gameObject);
                for (int i = 0; i < SlimeBlock_RedSpawnQuatity; i++)
                {
                    var enemy = enemyFactory.CreateEnemy(currentLevel, spawnPositionList);
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


public interface IEnemyFactory
{
    GameObject CreateEnemy(int currentLevel, List<float2> spawnPositionList);
}

public class GrimonkBrownFactory : ScriptableObject,IEnemyFactory
{
    GameObject player;
    GameObject Grimonk_Brown;
    GameObject enemySpawner;

    public GrimonkBrownFactory(GameObject player, GameObject Grimonk_Brown, GameObject enemySpawner)
    {
        this.player = player;
        this.Grimonk_Brown = Grimonk_Brown;
        this.enemySpawner = enemySpawner;
    }

    public GameObject CreateEnemy(int currentLevel, List<float2> spawnPositionList)
    {
        var enemy = Instantiate(Grimonk_Brown);
        int positionSelection = (enemy.GetComponent<EnemyControl>().random.NextInt(0, 23));
        enemy.transform.position = new float3(spawnPositionList[positionSelection].x, spawnPositionList[positionSelection].y, 0) + enemy.GetComponent<EnemyControl>().random.NextInt3(new int3(-4, -4, 0), new int3(4, 4, 0));
        enemy.transform.rotation = quaternion.identity;
        enemy.GetComponent<EnemyControl>().player = player;
        enemy.GetComponent<EnemyControl>().EnemySpawner = enemySpawner;
        enemy.GetComponent<EnemyControl>().currentHitPoint = Mathf.RoundToInt(enemy.GetComponent<EnemyControl>().maxHP + enemy.GetComponent<EnemyControl>().maxHP * (currentLevel - 1) * 0.1f);
        enemy.GetComponent<EnemyControl>().damage += Mathf.RoundToInt(currentLevel * 0.5f);
        return enemy;
    }
}

public class SlimeBlockRedFactory : ScriptableObject,IEnemyFactory
{
    GameObject player;
    GameObject SlimeBlock_Red;
    GameObject enemySpawner;

    public SlimeBlockRedFactory(GameObject player, GameObject SlimeBlock_Red, GameObject enemySpawner)
    {
        this.player = player;
        this.SlimeBlock_Red = SlimeBlock_Red;
        this.enemySpawner = enemySpawner;
    }
    public GameObject CreateEnemy(int currentLevel, List<float2> spawnPositionList)
    {
        var enemy = Instantiate(SlimeBlock_Red);
        int positionSelection = (enemy.GetComponent<EnemyControl>().random.NextInt(0, 23));
        enemy.transform.position = new float3(spawnPositionList[positionSelection].x, spawnPositionList[positionSelection].y, 0) + enemy.GetComponent<EnemyControl>().random.NextInt3(new int3(-4, -4, 0), new int3(4, 4, 0));
        enemy.transform.rotation = quaternion.identity;
        enemy.GetComponent<EnemyControl>().player = player;
        enemy.GetComponent<EnemyControl>().EnemySpawner = enemySpawner;
        enemy.GetComponent<EnemyControl>().currentHitPoint = Mathf.RoundToInt(enemy.GetComponent<EnemyControl>().maxHP + enemy.GetComponent<EnemyControl>().maxHP * (currentLevel - 1) * 0.1f);
        enemy.GetComponent<EnemyControl>().damage += Mathf.RoundToInt(currentLevel * 0.5f);
        return enemy;
    }
}
