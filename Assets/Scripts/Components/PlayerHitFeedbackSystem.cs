using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Collections;

[BurstCompile]
public partial struct PlayerHitFeedbackSystem : ISystem
{
    private float nextHitTime;

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (SystemAPI.TryGetSingletonEntity<PlayerInfoComponent>(out Entity entity))
        {
            if (state.EntityManager.HasComponent<PlayerAttackedFlag>(entity))
            {
                EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
                PlayerHPCalculate(state.EntityManager, entity, ecb, ref state);
                ecb.Playback(state.EntityManager);
                ecb.Dispose();
            }
        }

    }

    [BurstCompile]
    public void PlayerHPCalculate(EntityManager entityManager, Entity entity, EntityCommandBuffer ecb, ref SystemState state)
    {
        var playerInfo = entityManager.GetComponentData<PlayerInfoComponent>(entity);
        var hitInfo = entityManager.GetComponentData<PlayerAttackedFlag>(entity);

        playerInfo.currentHitPoint -= hitInfo.damage;
        ecb.SetComponent(entity, playerInfo);
        ecb.RemoveComponent<PlayerAttackedFlag>(entity);

    }
}
