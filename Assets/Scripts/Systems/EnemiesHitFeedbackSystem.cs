using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct EnemiesHitFeedbackSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BulletHitEnemyFlag>();
    }

    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        foreach(var (transform, enemyInfo, damage, entity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<EnemiesInfo>, RefRW<BulletHitEnemyFlag>>().WithEntityAccess())
        {
            enemyInfo.ValueRW.currentHitPoint-= damage.ValueRO.damage;
            ecb.RemoveComponent<BulletHitEnemyFlag>(entity);
            if(enemyInfo.ValueRO.currentHitPoint<=0)
            {
                var refData = state.EntityManager.GetComponentObject<VisualsReferenceComponent>(entity);
                refData.gameObject.GetComponent<DestroyVisualCommand>().DestroySelf();
                ecb.DestroyEntity(entity);
            }
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
