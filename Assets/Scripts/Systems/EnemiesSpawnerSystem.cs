using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;
using static UnityEngine.EventSystems.EventTrigger;
using Random = Unity.Mathematics.Random;

[BurstCompile]
public partial struct EnemiesSpawnerSystem : ISystem
{
    private Random random;
    private uint updateCount;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<SpawnerInfo>();
        random = Random.CreateFromIndex(updateCount++);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var enemiesQuery = SystemAPI.QueryBuilder().WithAll<EnemiesInfo>().Build();
        if (enemiesQuery.IsEmpty)
        {
            if (SystemAPI.TryGetSingleton(out SpawnerInfo enemiesSpawner))
            {
                foreach (SpawnComponentInfo spawnInfo in enemiesSpawner.spawnInfo)
                {
                    NativeArray<Entity> entityList = new NativeArray<Entity>(spawnInfo.spawnQuantities, Allocator.Temp);
                    state.EntityManager.Instantiate(spawnInfo.enemiesPrefab, entityList);
                    foreach (var entity in entityList)
                    {
                        var enemy = SystemAPI.GetComponentRW<EnemiesInfo>(entity);
                        enemy.ValueRW.random = Random.CreateFromIndex((uint)entity.Index);
                    }
                    entityList.Dispose();
                }

               
                var bigFishSpawnJob = new FishSpawnJob
                {
                    spawnerInfo = enemiesSpawner,
                };
                bigFishSpawnJob.ScheduleParallel();
                state.Dependency.Complete();
            }
        }
    }

    [BurstCompile]
    public partial struct FishSpawnJob : IJobEntity
    {
        public SpawnerInfo spawnerInfo;


        readonly void Execute([EntityIndexInQuery] int index, ref LocalTransform transform, ref EnemiesInfo enemy, Entity entity)
        {
            float2 newPosition = (enemy.random.NextFloat2(new float2(-40, -20), new float2(40, 20)));

            transform.Position = new float3(newPosition.x, newPosition.y, 0);
            transform.Rotation = quaternion.identity;      
        }
    }

}


