using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


[BurstCompile]
public partial struct PlayerPickHealthSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (SystemAPI.TryGetSingletonEntity<PlayerInfoComponent>(out Entity player))
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (transform, health, entity) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<HealthComponent>>().WithEntityAccess())
            {
              
                if (math.distance(state.EntityManager.GetComponentData<LocalTransform>(player).Position, transform.ValueRO.Position) <= 1f)
                {
                    var playerInfo = state.EntityManager.GetComponentData<PlayerInfoComponent>(player);
                    playerInfo.currentHitPoint += Mathf.RoundToInt(playerInfo.maxHitPoint * health.ValueRO.healPercentage);
                    if (playerInfo.currentHitPoint > playerInfo.maxHitPoint)
                    {
                        playerInfo.currentHitPoint = playerInfo.maxHitPoint;
                    }
                    ecb.SetComponent(player, playerInfo);
                    ecb.DestroyEntity(entity);
                }
              
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}
