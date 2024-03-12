using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct EnemyAnimationSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.ManagedAPI.TryGetSingleton(out AnimationVisualsPrefabs animationVisualsPrefabs))
        {
            return;
        }

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (transform, enemyInfo, entity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<EnemiesInfo>>().WithEntityAccess())
        {
            if (!state.EntityManager.HasComponent<VisualsReferenceComponent>(entity))
            {

                GameObject enemyVisual = Object.Instantiate(animationVisualsPrefabs.VisualPrefab[enemyInfo.ValueRO.enemiesType]);
               
                ecb.AddComponent(entity, new VisualsReferenceComponent { gameObject = enemyVisual });
            }
            else
            {
                VisualsReferenceComponent enemyVisualRef = state.EntityManager.GetComponentData<VisualsReferenceComponent>(entity);
                enemyVisualRef.gameObject.transform.position = transform.ValueRO.Position;
                enemyVisualRef.gameObject.transform.rotation = transform.ValueRO.Rotation;
                if (state.EntityManager.HasComponent<EnemyMovementInfo>(entity))
                {
                    EnemyMovementInfo enemyMovementInfo = state.EntityManager.GetComponentData<EnemyMovementInfo>(entity);
                    enemyVisualRef.gameObject.GetComponent<Animator>().SetFloat("Speed", enemyMovementInfo.moveSpeed);
                }

            }
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
