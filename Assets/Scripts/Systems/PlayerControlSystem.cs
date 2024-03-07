using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.EventTrigger;

[BurstCompile]
public partial struct PlayerControlSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerInfoComponent>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");
        var input = new float3(horizontalInput, verticalInput, 0) * SystemAPI.Time.DeltaTime;

        

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (transform, player, entity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<PlayerInfoComponent>>().WithEntityAccess())
        {
            float maxSpeed = 1f;

            if (Input.GetKey(KeyCode.Space))
            {
                maxSpeed = 2f;
            }
            else
            {
                maxSpeed = 1f;
                if(player.ValueRW.Speed > maxSpeed)
                {
                    player.ValueRW.Speed -= player.ValueRO.SpeedDecelerator;
                }
            }
            if (input.x != 0 || input.y != 0)
            {
                if (player.ValueRW.Speed < (maxSpeed - player.ValueRO.SpeedAccelerate))
                {
                    player.ValueRW.Speed += player.ValueRO.SpeedAccelerate;
                }
            }
            else if (input.x == 0 && input.y == 0)
            {
                if (player.ValueRW.Speed >0)
                {
                    player.ValueRW.Speed -= player.ValueRO.SpeedDecelerator;
                    if(player.ValueRW.Speed <= player.ValueRO.SpeedDecelerator)
                    {
                        player.ValueRW.Speed = 0;
                    }
                }
            }

            float3 move = new float3();

            if(input.x==0 && input.y==0)
            {
                if (state.EntityManager.HasComponent<PlayerMovementInfo>(entity))
                {
                    float2 lastMove = state.EntityManager.GetComponentData<PlayerMovementInfo>(entity).moveDirection;
                    move.x = lastMove.x;
                    move.y = lastMove.y;
                }
            }
            else
            {
                move.x = input.x;
                move.y = input.y;
            }
            
            float3 newPosition = transform.ValueRW.Position + move * player.ValueRO.Speed;
            transform.ValueRW.Position = newPosition;
            if (input.x > 0)
            {
                transform.ValueRW.Rotation = new quaternion(0, 0, 0, 1);
            }
            else
                if (input.x < 0)
            {
                transform.ValueRW.Rotation = new quaternion(0, -6, 0, 1);
            }
         
            if (!state.EntityManager.HasComponent<PlayerMovementInfo>(entity))
            {
                ecb.AddComponent(entity, new PlayerMovementInfo
                {
                    moveDirection = new float2(input.x, input.y),
                    moveSpeed = player.ValueRO.Speed
                });
            }
            else
            {
                if (input.x != 0 || input.y != 0)
                {
                    state.EntityManager.SetComponentData(entity, new PlayerMovementInfo
                    {
                        moveDirection = new float2(input.x, input.y),
                        moveSpeed = player.ValueRO.Speed
                    });
                }
                else
                {
                    if(player.ValueRW.Speed ==0)
                    {
                        state.EntityManager.SetComponentData(entity, new PlayerMovementInfo
                        {
                            moveDirection = new float2(0, 0),
                            moveSpeed = player.ValueRO.Speed
                        });
                    }
                }
                

            }
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
