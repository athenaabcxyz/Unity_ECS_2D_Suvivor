using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Transforms;
using Unity.Burst;
using Unity.Jobs;
using static UnityEngine.RuleTile.TilingRuleOutput;
using System;

[BurstCompile]
public partial struct EnemyAttackSystem : ISystem
{
    private float nextHitTime;

    [BurstCompile]
    void OnCreate(ref SystemState state)
    {
        nextHitTime = 0f;
    }
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if(SystemAPI.TryGetSingletonEntity<PlayerInfoComponent>(out Entity player))
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            var playerPosition = state.EntityManager.GetComponentData<LocalTransform>(player).Position;
            foreach (var (transform, enemy, entity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<EnemiesInfo>>().WithEntityAccess())
            {
                if (SystemAPI.Time.ElapsedTime > nextHitTime)
                {
                    if (math.distance(transform.ValueRW.Position, playerPosition) <= 0.4f)
                    {
                        if(!state.EntityManager.HasComponent<PlayerAttackedFlag>(player))
                        {
                            ecb.AddComponent(player, new PlayerAttackedFlag
                            {
                                damage = enemy.ValueRO.damage
                            });
                            nextHitTime = (float)SystemAPI.Time.ElapsedTime + state.EntityManager.GetComponentData<PlayerInfoComponent>(player).hitCoolDown;
                        }                      
                    }
                }
                    
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
       
    }


}
