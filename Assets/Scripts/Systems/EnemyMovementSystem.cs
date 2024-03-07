using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Burst;
using UnityEngine.Windows;

[BurstCompile]
public partial struct EnemyMovementSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EnemiesInfo>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var playerEntity = SystemAPI.GetSingletonEntity<PlayerInfoComponent>();
        var enemyMovementJob = new EnemyChasePlayerJob
        {
            currentPlayerPosition = state.EntityManager.GetComponentData<LocalTransform>(playerEntity).Position,
            deltaTime = SystemAPI.Time.DeltaTime
        };
        enemyMovementJob.ScheduleParallel();
    }

    [BurstCompile]
    public partial struct EnemyChasePlayerJob: IJobEntity
    {
        public float3 currentPlayerPosition;
        public float deltaTime;
        readonly void Execute(ref LocalTransform transform, ref EnemiesInfo info)
        {
            float3 direction = currentPlayerPosition - transform.Position;
            transform.Position += math.normalize(direction) * info.moveSpeed * deltaTime;
            if (direction.x > 0)
            {
                transform.Rotation = new quaternion(0, 0, 0, 1);
            }
            else
             if (direction.x < 0)
            {
                transform.Rotation = new quaternion(0, -6, 0, 1);
            }
        }
    }
}
