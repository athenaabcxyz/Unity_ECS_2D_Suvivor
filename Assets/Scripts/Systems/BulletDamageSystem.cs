using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;

public partial struct BulletDamageSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BulletMovementInfo>();
    }

    public void OnUpdate(ref SystemState state)
    {

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach (var (transform, info, bullet) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<BulletInfo>>().WithEntityAccess())
        {
            Entity closetEnemy = Entity.Null;
            float smalestDistance = 0.8f;
            bool isCollided = false;

            foreach (var (transformEnemy, enemyInfo, enemy) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<EnemiesInfo>>().WithEntityAccess())
            {
                if (math.distance(transform.ValueRO.Position, transformEnemy.ValueRO.Position) <= smalestDistance)
                {
                    closetEnemy = enemy;
                    isCollided = true;
                    smalestDistance = math.distance(transform.ValueRO.Position, transformEnemy.ValueRO.Position);
                }
            }

            if (isCollided && closetEnemy!=Entity.Null)
            {
                ecb.DestroyEntity(bullet);
                ecb.AddComponent(closetEnemy, new BulletHitEnemyFlag
                {
                    damage = info.ValueRO.deliveryDamage
                });
            }
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }


}
