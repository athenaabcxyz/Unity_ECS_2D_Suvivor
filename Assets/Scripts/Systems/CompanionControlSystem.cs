using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct CompanionControlSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<CompanionMovementInfo>();
        state.RequireForUpdate<PlayerInfoComponent>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state) 
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);
        if (SystemAPI.TryGetSingletonEntity<PlayerInfoComponent>(out Entity entity))
        {
            var playerPosition = state.EntityManager.GetComponentData<LocalTransform>(entity).Position;
            NativeList<float3> companionPosition = new NativeList<float3>(Allocator.Temp);
            foreach (var (transform, companion) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<CompanionInfo>>())
            {
                companionPosition.Add(transform.ValueRO.Position);
            }

            NativeArray<float3> nativePosition = companionPosition.ToArray(Allocator.Persistent);
            var handler = new CompanionMovementJob
            {
                companionPosition = nativePosition,
                playerPosition = playerPosition,
                deltaTime = SystemAPI.Time.DeltaTime,
                ecb = ecb.AsParallelWriter()
            }.ScheduleParallel(new Unity.Jobs.JobHandle());
            handler.Complete();
            companionPosition.Dispose();
            nativePosition.Dispose();

        }
        
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    [BurstCompile]
    public partial struct CompanionMovementJob: IJobEntity
    {
        [ReadOnly] public NativeArray<float3> companionPosition;
        public float3 playerPosition;
        public float deltaTime;
        public EntityCommandBuffer.ParallelWriter ecb;

        [BurstCompile]
        public void Execute(ref LocalTransform transform, ref CompanionInfo companion, ref CompanionMovementInfo movementInfo, ref CurrentWeaponInfo weapon)
        {
            float3 direction = playerPosition - transform.Position;
            float3 move = math.normalize(direction) * companion.Speed * deltaTime;

            float3 avoidForce = float3.zero;

            foreach (float3 otherCompanion in companionPosition)
            {

                float3 dir = transform.Position + move - otherCompanion;
                var dist = math.distance(transform.Position + move, otherCompanion);
                if (dist <= 2f)
                {
                    avoidForce += dir / dist;
                }
            }

            if (direction.x*direction.x + direction.y * direction.y >= 4f)
            {
                transform.Position += move + math.normalize(avoidForce) * deltaTime * companion.Speed;
                movementInfo.moveDirection = move + math.normalize(avoidForce) * deltaTime * companion.Speed;
            }
            else
            {
                movementInfo.moveDirection = new float3(0, 0, 0);
            }
        }   
    }   
}
