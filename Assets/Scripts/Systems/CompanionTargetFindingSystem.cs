using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct CompanionTargetFindingSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<CompanionMovementInfo>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {

        foreach (var (transform, currentTarget, companion) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<CurrentTarget>, RefRO<CompanionInfo>>())
        {
            float closetDistance = companion.ValueRO.range;
            Entity closetEnemy = Entity.Null;

            foreach (var (transformEnemy, enemy, entityEnemy) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<EnemiesInfo>>().WithEntityAccess())
            {
                if (math.distance(transform.ValueRO.Position, transformEnemy.ValueRO.Position) <= closetDistance)
                {
                    closetDistance = math.distance(transform.ValueRO.Position, transformEnemy.ValueRO.Position);
                    closetEnemy = entityEnemy;
                }
            }
            if (closetEnemy != Entity.Null)
            {
                currentTarget.ValueRW.isAllowedToShoot = true;
                currentTarget.ValueRW.currentTarget = state.EntityManager.GetComponentData<LocalTransform>(closetEnemy).Position;
            }
            else
            {
                currentTarget.ValueRW.isAllowedToShoot = false;
            }

        }
    } 
}
