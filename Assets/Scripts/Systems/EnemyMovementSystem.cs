using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Burst;
using UnityEngine.Windows;
using static UnityEngine.EventSystems.EventTrigger;
using Unity.Collections;

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
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
        EntityCommandBuffer ecbMain = new EntityCommandBuffer(Allocator.Temp);
        foreach(var (transform, enemy, entity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<EnemiesInfo>>().WithEntityAccess())
        {
            if(!state.EntityManager.HasComponent<EnemyMovementInfo>(entity))
            {
                ecbMain.AddComponent(entity, new EnemyMovementInfo());
            }
        }
        ecbMain.Playback(state.EntityManager);
        ecbMain.Dispose();
        var ecbParalell = ecb.AsParallelWriter();
        var enemyMovementJob = new EnemyChasePlayerJob
        {
            currentPlayerPosition = state.EntityManager.GetComponentData<LocalTransform>(playerEntity).Position,
            deltaTime = SystemAPI.Time.DeltaTime,
            ecb = ecbParalell,
        };
        enemyMovementJob.ScheduleParallel();
        state.Dependency.Complete();
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    [BurstCompile]
    public partial struct EnemyChasePlayerJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ecb;
        public float3 currentPlayerPosition;
        public float deltaTime;

        readonly void Execute([EntityIndexInQuery] int index, ref LocalTransform transform, ref EnemiesInfo info, Entity entity, ref EnemyMovementInfo movementInfo)
        {
            float3 direction = currentPlayerPosition - transform.Position;
            float3 move = math.normalize(direction) * info.moveSpeed * deltaTime;
            transform.Position += move;

            ecb.SetComponent(index, entity, new EnemyMovementInfo
            {
                moveDirection = new float2(move.x, move.y),
                mouseAngle = GetAimDirection(GetAngleFromVector(direction)),
                moveSpeed = info.moveSpeed,
            });

            [BurstCompile]
            RotationEnum GetAimDirection(float angleDegrees)
            {
                RotationEnum aimDirection;

                // Set player direction
                //Up Right
                if (angleDegrees >= 22f && angleDegrees <= 67f)
                {
                    aimDirection = RotationEnum.aimUpRight;
                }
                // Up
                else if (angleDegrees > 67f && angleDegrees <= 112f)
                {
                    aimDirection = RotationEnum.aimUp;
                }
                // Up Left
                else if (angleDegrees > 112f && angleDegrees <= 158f)
                {
                    aimDirection = RotationEnum.aimUpLeft;
                }
                // Left
                else if ((angleDegrees <= 180f && angleDegrees > 158f) || (angleDegrees > -180 && angleDegrees <= -135f))
                {
                    aimDirection = RotationEnum.aimLeft;
                }
                // Down
                else if ((angleDegrees > -135f && angleDegrees <= -45f))
                {
                    aimDirection = RotationEnum.aimDown;
                }
                // Right
                else if ((angleDegrees > -45f && angleDegrees <= 0f) || (angleDegrees > 0 && angleDegrees < 22f))
                {
                    aimDirection = RotationEnum.aimRight;
                }
                else
                {
                    aimDirection = RotationEnum.aimRight;
                }

                return aimDirection;

            }

            float GetAngleFromVector(float3 vector)
            {

                float radians = Mathf.Atan2(vector.y, vector.x);

                float degrees = radians * Mathf.Rad2Deg;

                return degrees;

            }
        }
    }

}
