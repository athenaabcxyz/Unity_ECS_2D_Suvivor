using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;
using Unity.Burst;

[BurstCompile]
public partial struct BulletDamageSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BulletMovementInfo>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach (var (transform, info, health, bullet) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<BulletInfo>, RefRO<HealthOnBulletInfo>>().WithEntityAccess())
        {
            Entity closetEnemy = Entity.Null;
            float smalestDistance = 0.8f;
            bool isCollided = false;
            if (!info.ValueRO.isEnemyBullet)
            {
                foreach (var (transformEnemy, enemyInfo, enemy) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<EnemiesInfo>>().WithEntityAccess())
                {
                    if (math.distance(transform.ValueRO.Position, transformEnemy.ValueRO.Position) <= smalestDistance)
                    {
                        closetEnemy = enemy;
                        isCollided = true;
                        smalestDistance = math.distance(transform.ValueRO.Position, transformEnemy.ValueRO.Position);
                    }
                }

                if (isCollided && closetEnemy != Entity.Null)
                {
                    ecb.DestroyEntity(bullet);
                    ecb.AddComponent(closetEnemy, new BulletHitFlag
                    {
                        damage = info.ValueRO.deliveryDamage,
                        healthPrefab = health.ValueRO.healthPrefab,
                    });
                }
            }
            else
            {
                if (SystemAPI.TryGetSingletonEntity<PlayerInfoComponent>(out var player))
                {
                    var transformPlayer = state.EntityManager.GetComponentData<LocalTransform>(player);
                    if (math.distance(transform.ValueRO.Position, transformPlayer.Position) <= smalestDistance)
                    {
                        closetEnemy = player;
                        isCollided = true;
                        smalestDistance = math.distance(transform.ValueRO.Position, transformPlayer.Position);
                    }
                }


                if (isCollided && closetEnemy != Entity.Null)
                {
                    ecb.DestroyEntity(bullet);
                    ecb.AddComponent(closetEnemy, new BulletHitFlag
                    {
                        damage = info.ValueRO.deliveryDamage,
                        healthPrefab = health.ValueRO.healthPrefab,
                    });
                }
            }

        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }


}
