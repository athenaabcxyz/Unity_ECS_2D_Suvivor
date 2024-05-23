using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

[BurstCompile]
public partial struct EnemiesSpawnerSystem : ISystem
{
    private int currentLevel;
    private Random random;
    private uint updateCount;
    private NativeArray<float2> spawnPositionList;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        currentLevel = 0;
        state.RequireForUpdate<SpawnerInfo>();
        spawnPositionList = new NativeArray<float2>(24, Allocator.Persistent);
        spawnPositionList[0] = new float2(-60, 35);
        spawnPositionList[1] = new float2(-45, 35);
        spawnPositionList[2] = new float2(-30, 35);
        spawnPositionList[3] = new float2(-15, 35);
        spawnPositionList[4] = new float2(0, 35);
        spawnPositionList[5] = new float2(15, 35);
        spawnPositionList[6] = new float2(30, 35);
        spawnPositionList[7] = new float2(45, 35);
        spawnPositionList[8] = new float2(60, 35);
        spawnPositionList[9] = new float2(-60, -35);
        spawnPositionList[10] = new float2(-45, -35);
        spawnPositionList[11] = new float2(-30, -35);
        spawnPositionList[12] = new float2(-15, -35);
        spawnPositionList[13] = new float2(0, -35);
        spawnPositionList[14] = new float2(15, -35);
        spawnPositionList[15] = new float2(30, -35);
        spawnPositionList[16] = new float2(45, -35);
        spawnPositionList[17] = new float2(60, -35);
        spawnPositionList[18] = new float2(-60, 17.5f);
        spawnPositionList[19] = new float2(-60, 0);
        spawnPositionList[20] = new float2(60, -17.5f);
        spawnPositionList[21] = new float2(60, 17.5f);
        spawnPositionList[22] = new float2(60, 0);

        random = Random.CreateFromIndex(updateCount++);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var enemiesQuery = SystemAPI.QueryBuilder().WithAll<EnemiesInfo>().Build();
        if (enemiesQuery.IsEmpty)
        {
            currentLevel += 1;
            if (SystemAPI.TryGetSingleton(out SpawnerInfo enemiesSpawner))
            {
                if (enemiesSpawner.Grimonk_BrownSpawnQuatity > 0)
                {
                    var Grimonk_BrownArray = new NativeArray<Entity>(Mathf.RoundToInt(enemiesSpawner.Grimonk_BrownSpawnQuatity*currentLevel*1.25f), Allocator.Temp);
                    state.EntityManager.Instantiate(enemiesSpawner.Grimonk_Brown, Grimonk_BrownArray);
                    foreach (var entity in Grimonk_BrownArray)
                    {
                        
                        var enemy = SystemAPI.GetComponentRW<EnemiesInfo>(entity);
                        enemy.ValueRW.random = Random.CreateFromIndex((uint)entity.Index);
                    }
                }

                if (enemiesSpawner.Hedusa_BlueSpawnQuatity > 0)
                {
                    var Hedusa_BlueArray = new NativeArray<Entity>(Mathf.RoundToInt(enemiesSpawner.Hedusa_BlueSpawnQuatity*currentLevel*1.25f), Allocator.Temp);
                    state.EntityManager.Instantiate(enemiesSpawner.Hedusa_Blue, Hedusa_BlueArray);
                    foreach (var entity in Hedusa_BlueArray)
                    {
                        var enemy = SystemAPI.GetComponentRW<EnemiesInfo>(entity);
                        enemy.ValueRW.random = Random.CreateFromIndex((uint)entity.Index);
                    }
                }

                if (enemiesSpawner.Hedusa_GreenSpawnQuatity > 0)
                {
                    var Hedusa_GreenArray = new NativeArray<Entity>(Mathf.RoundToInt(enemiesSpawner.Hedusa_GreenSpawnQuatity * currentLevel * 1.25f), Allocator.Temp);
                    state.EntityManager.Instantiate(enemiesSpawner.Hedusa_Green, Hedusa_GreenArray);
                    foreach (var entity in Hedusa_GreenArray)
                    {
                        var enemy = SystemAPI.GetComponentRW<EnemiesInfo>(entity);
                        enemy.ValueRW.random = Random.CreateFromIndex((uint)entity.Index);
                    }
                }

                if (enemiesSpawner.Hedusa_RedSpawnQuatity > 0)
                {
                    var Hedusa_RedArray = new NativeArray<Entity>(Mathf.RoundToInt(enemiesSpawner.Hedusa_RedSpawnQuatity * currentLevel * 1.25f), Allocator.Temp);
                    state.EntityManager.Instantiate(enemiesSpawner.Hedusa_Red, Hedusa_RedArray);
                    foreach (var entity in Hedusa_RedArray)
                    {
                        var enemy = SystemAPI.GetComponentRW<EnemiesInfo>(entity);
                        enemy.ValueRW.random = Random.CreateFromIndex((uint)entity.Index);
                    }
                }

                if (enemiesSpawner.MudRock_BrownSpawnQuatity > 0)
                {
                    var Array = new NativeArray<Entity>(Mathf.RoundToInt(enemiesSpawner.MudRock_BrownSpawnQuatity * currentLevel * 1.25f), Allocator.Temp);
                    state.EntityManager.Instantiate(enemiesSpawner.MudRock_Brown, Array);
                    foreach (var entity in Array)
                    {
                        var enemy = SystemAPI.GetComponentRW<EnemiesInfo>(entity);
                        enemy.ValueRW.random = Random.CreateFromIndex((uint)entity.Index);
                    }
                }

                if (enemiesSpawner.Orc_FleshSpawnQuatity > 0)
                {
                    var Array = new NativeArray<Entity>(Mathf.RoundToInt(enemiesSpawner.Orc_FleshSpawnQuatity + enemiesSpawner.Orc_FleshSpawnQuatity * (currentLevel-1) * 0.25f), Allocator.Temp);
                    state.EntityManager.Instantiate(enemiesSpawner.Orc_Flesh, Array);
                    foreach (var entity in Array)
                    {
                        var enemy = SystemAPI.GetComponentRW<EnemiesInfo>(entity);
                        enemy.ValueRW.random = Random.CreateFromIndex((uint)entity.Index);
                    }
                }

                if (enemiesSpawner.SlimeBlock_BlueSpawnQuatity > 0)
                {
                    var Array = new NativeArray<Entity>(Mathf.RoundToInt(enemiesSpawner.SlimeBlock_BlueSpawnQuatity + enemiesSpawner.SlimeBlock_BlueSpawnQuatity * (currentLevel-1) * 0.25f), Allocator.Temp);
                    state.EntityManager.Instantiate(enemiesSpawner.SlimeBlock_Blue, Array);
                    foreach (var entity in Array)
                    {
                        var enemy = SystemAPI.GetComponentRW<EnemiesInfo>(entity);
                        enemy.ValueRW.random = Random.CreateFromIndex((uint)entity.Index);
                    }
                }

                if (enemiesSpawner.SlimeBlock_GreenSpawnQuatity > 0)
                {
                    var Array = new NativeArray<Entity>(Mathf.RoundToInt(enemiesSpawner.SlimeBlock_GreenSpawnQuatity + enemiesSpawner.SlimeBlock_GreenSpawnQuatity * (currentLevel-1) * 0.25f), Allocator.Temp);
                    state.EntityManager.Instantiate(enemiesSpawner.SlimeBlock_Green, Array);
                    foreach (var entity in Array)
                    {
                        var enemy = SystemAPI.GetComponentRW<EnemiesInfo>(entity);
                        enemy.ValueRW.random = Random.CreateFromIndex((uint)entity.Index);
                    }
                }

                if (enemiesSpawner.SlimeBlock_RedSpawnQuatity > 0)
                {
                    var Array = new NativeArray<Entity>(Mathf.RoundToInt(enemiesSpawner.SlimeBlock_RedSpawnQuatity + enemiesSpawner.SlimeBlock_RedSpawnQuatity * (currentLevel-1) * 0.25f), Allocator.Temp);
                    state.EntityManager.Instantiate(enemiesSpawner.SlimeBlock_Red, Array);
                    foreach (var entity in Array)
                    {
                        var enemy = SystemAPI.GetComponentRW<EnemiesInfo>(entity);
                        enemy.ValueRW.random = Random.CreateFromIndex((uint)entity.Index);
                    }
                }

                if (enemiesSpawner.SlizzardSpawnQuatity > 0)
                {
                    var Array = new NativeArray<Entity>(Mathf.RoundToInt(enemiesSpawner.SlizzardSpawnQuatity + enemiesSpawner.SlizzardSpawnQuatity * (currentLevel-1) * 0.25f), Allocator.Temp);
                    state.EntityManager.Instantiate(enemiesSpawner.Slizzard, Array);
                    foreach (var entity in Array)
                    {
                        var enemy = SystemAPI.GetComponentRW<EnemiesInfo>(entity);
                        enemy.ValueRW.random = Random.CreateFromIndex((uint)entity.Index);
                    }
                }

                var bigFishSpawnJob = new FishSpawnJob
                {
                    spawnerInfo = enemiesSpawner,
                    spawnPositionList = spawnPositionList,
                    currentLevel = currentLevel,
                }.ScheduleParallel(new Unity.Jobs.JobHandle());
                bigFishSpawnJob.Complete();
            }
        }
    }

    [BurstCompile]
    public partial struct FishSpawnJob : IJobEntity
    {
        [ReadOnly] public int currentLevel;
        public SpawnerInfo spawnerInfo;
        [ReadOnly ]public NativeArray<float2> spawnPositionList;

        readonly void Execute(ref LocalTransform transform, ref EnemiesInfo enemy, Entity entity)
        {
            int positionSelection = (enemy.random.NextInt(0, 23));

            transform.Position = new float3(spawnPositionList[positionSelection].x, spawnPositionList[positionSelection].y, 0) + enemy.random.NextInt3(new int3(-4,-4, 0), new int3(4,4,0));
            transform.Rotation = quaternion.identity;
            enemy.currentHitPoint = Mathf.RoundToInt(enemy.maxHP + enemy.maxHP*(currentLevel-1) * 0.1f);
            enemy.damage += Mathf.RoundToInt(currentLevel * 0.5f);
        }
    }

}


