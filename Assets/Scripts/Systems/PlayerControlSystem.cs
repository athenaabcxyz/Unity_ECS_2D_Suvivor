using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct PlayerControlSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerInfoComponent>();
    }

    public void OnUpdate(ref SystemState state)
    {
        
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");
        var mousePosition = Input.mousePosition;
        var input = math.normalizesafe(new float3(horizontalInput, verticalInput, 0)) * SystemAPI.Time.DeltaTime;

        

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (transform, player, entity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<PlayerInfoComponent>>().WithEntityAccess())
        {


            float3 movement = input * player.ValueRO.Speed;
            if (transform.ValueRO.Position.x+movement.x <= -59 || transform.ValueRO.Position.x+movement.x >= 59)
            {
                transform.ValueRW.Position += new float3(0, movement.y, 0);
            }
            else

                if (transform.ValueRO.Position.y+movement.y <= -32 || transform.ValueRO.Position.y + movement.y >= 32)
            {
                transform.ValueRW.Position += new float3(movement.x, 0, 0);
            }
            else
            {
                transform.ValueRW.Position += movement;
            }



            Vector3 dir = mousePosition - Camera.main.WorldToScreenPoint(transform.ValueRO.Position);

            if (!state.EntityManager.HasComponent<PlayerMovementInfo>(entity))
            {
                ecb.AddComponent(entity, new PlayerMovementInfo
                {
                    mouseAngle = GetAimDirection(GetAngleFromVector(dir)),
                    moveSpeed = player.ValueRO.Speed,
                    mousePosition = new float3(mousePosition.x, mousePosition.y, mousePosition.z)
                });
            }
            else
            {

                state.EntityManager.SetComponentData(entity, new PlayerMovementInfo
                {
                    moveDirection = new float2(input.x, input.y),
                    mouseAngle = GetAimDirection(GetAngleFromVector(dir)),
                    moveSpeed = player.ValueRO.Speed,
                    mousePosition = new float3(mousePosition.x, mousePosition.y, mousePosition.z)
                });



            }
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    [BurstCompile]
    public RotationEnum GetAimDirection(float angleDegrees)
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
    public float GetAngleFromVector(float3 vector)
    {
        
        float radians = Mathf.Atan2(vector.y, vector.x);

        float degrees = radians * Mathf.Rad2Deg;

        return degrees;

    }
}
