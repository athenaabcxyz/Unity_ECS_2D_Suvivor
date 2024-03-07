using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;

public partial struct PlayerAnimationSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        if(!SystemAPI.ManagedAPI.TryGetSingleton(out AnimationVisualsPrefabs animationVisualsPrefabs))
        {
            return;
        }

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach(var(transform, playerInfo, entity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<PlayerInfoComponent>>().WithEntityAccess())
        {
            if(!state.EntityManager.HasComponent<VisualsReferenceComponent>(entity))
            {
                GameObject playerVisual = Object.Instantiate(animationVisualsPrefabs.Player);
                ecb.AddComponent(entity, new VisualsReferenceComponent { gameObject = playerVisual });
            }
            else
            {
                VisualsReferenceComponent playerVisualRef = state.EntityManager.GetComponentData<VisualsReferenceComponent>(entity);
                playerVisualRef.gameObject.transform.position = transform.ValueRO.Position;
                playerVisualRef.gameObject.transform.rotation = transform.ValueRO.Rotation;
                float2 staticPosition = new float2(0, 0);
                if (state.EntityManager.HasComponent<PlayerMovementInfo>(entity))
                {
                    PlayerMovementInfo playerMovementInfo = state.EntityManager.GetComponentData<PlayerMovementInfo>(entity);
                    playerVisualRef.gameObject.GetComponent<Animator>().SetFloat("Speed", playerMovementInfo.moveSpeed);
                }
                   
            }
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}