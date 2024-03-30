using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public partial struct EnemiesHitFeedbackSystem : ISystem
{
    private Random random;
    public void OnCreate(ref SystemState state)
    {
        random = Random.CreateFromIndex(0);
        state.RequireForUpdate<BulletHitEnemyFlag>();
    }

    public void OnUpdate(ref SystemState state)
    {
        if(SystemAPI.TryGetSingletonEntity<PlayerInfoComponent>(out Entity player))
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
            foreach (var (transform, enemyInfo, damage, entity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<EnemiesInfo>, RefRW<BulletHitEnemyFlag>>().WithEntityAccess())
            {
                enemyInfo.ValueRW.currentHitPoint -= damage.ValueRO.damage;

                if (enemyInfo.ValueRO.currentHitPoint <= 0)
                {
                    var levelInfo = state.EntityManager.GetComponentData<LevelingInfoComponent>(player);
                    var refData = state.EntityManager.GetComponentObject<VisualsReferenceComponent>(entity);
                    SystemAPI.ManagedAPI.TryGetSingleton<AnimationVisualsPoolList>(out AnimationVisualsPoolList poolList);
                    refData.gameObject.SetActive(false);
                    float chance = random.NextFloat(0, 100);
                    if (chance > 0 && chance <= 3)
                    {
                        var health = state.EntityManager.Instantiate(damage.ValueRO.healthPrefab);
                        state.EntityManager.SetComponentData(health, new LocalTransform
                        {
                            Position = transform.ValueRO.Position,
                            Rotation = quaternion.identity,
                            Scale = 1
                        });
                    }
                    levelInfo.currentExp += levelInfo.currentLevel;
                    ecb.SetComponent(player, levelInfo);
                    poolList.VisualPools[enemyInfo.ValueRO.enemiesType - 1].VisualPrefabPool.Add(refData.gameObject);
                    ecb.DestroyEntity(entity);
                }
                else
                {
                    ecb.RemoveComponent<BulletHitEnemyFlag>(entity);
                }

            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
       
    }
}
