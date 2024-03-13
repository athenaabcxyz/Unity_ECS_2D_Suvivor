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
                GameObject playerVisual = Object.Instantiate(animationVisualsPrefabs.VisualPrefab[0]);
                ecb.AddComponent(entity, new VisualsReferenceComponent { gameObject = playerVisual });
            }
            else
            {
                VisualsReferenceComponent playerVisualRef = state.EntityManager.GetComponentData<VisualsReferenceComponent>(entity);
                playerVisualRef.gameObject.transform.position = transform.ValueRO.Position;
                playerVisualRef.gameObject.transform.rotation = transform.ValueRO.Rotation;
                if (state.EntityManager.HasComponent<PlayerMovementInfo>(entity))
                {
                    PlayerMovementInfo playerMovementInfo = state.EntityManager.GetComponentData<PlayerMovementInfo>(entity);
                    if(playerMovementInfo.moveDirection.x!=0 || playerMovementInfo.moveDirection.y!=0)
                    {
                        playerVisualRef.gameObject.GetComponent<Animator>().SetBool("isMoving", true);
                        playerVisualRef.gameObject.GetComponent<Animator>().SetBool("isIdle", false);

                    }
                    else
                    {
                        playerVisualRef.gameObject.GetComponent<Animator>().SetBool("isMoving", false);
                        playerVisualRef.gameObject.GetComponent<Animator>().SetBool("isIdle", true);
                    }
                    var playerWeaponBehaviorComponent = playerVisualRef.gameObject.GetComponent<PlayerWeaponBehavior>();
                    var weaponShootPosition = (float3) playerWeaponBehaviorComponent.GetShootPosition();
                    var weaponPosition = (float3) playerWeaponBehaviorComponent.GetWeaponPosition();    
                    var weaponDirection = playerMovementInfo.mousePosition - (float3)Camera.main.WorldToScreenPoint(weaponPosition);
                    playerWeaponBehaviorComponent.Aim(RotationEnum.aimUp, GetAngleFromVector(weaponDirection));
                    if (!state.EntityManager.HasComponent<CurrentWeaponInfo>(entity))
                    {
                        ecb.AddComponent(entity, new CurrentWeaponInfo
                        {
                            weaponShootPosition = weaponShootPosition,
                            weaponShootDirection = weaponDirection,
                        });
                    }
                    else
                    {
                        ecb.SetComponent(entity, new CurrentWeaponInfo
                        {
                            weaponShootPosition = weaponShootPosition,
                            weaponShootDirection = weaponDirection,
                        });
                    }
                    switch (playerMovementInfo.mouseAngle)
                    {
                        case RotationEnum.aimUp:
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUp", true);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimDown", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimRight", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimLeft", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpLeft", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpRight", false);
                            playerWeaponBehaviorComponent.Aim(RotationEnum.aimUp, GetAngleFromVector(weaponDirection));
                            break;
                        case RotationEnum.aimDown:
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUp", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimDown", true);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimRight", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimLeft", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpLeft", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpRight", false);
                            playerWeaponBehaviorComponent.Aim(RotationEnum.aimDown, GetAngleFromVector(weaponDirection));
                            break;
                        case RotationEnum.aimRight:
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUp", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimDown", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimRight", true);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimLeft", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpLeft", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpRight", false);
                            playerWeaponBehaviorComponent.Aim(RotationEnum.aimRight, GetAngleFromVector(weaponDirection));
                            break;
                        case RotationEnum.aimLeft:
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUp", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimDown", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimRight", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimLeft", true);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpLeft", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpRight", false);
                            playerWeaponBehaviorComponent.Aim(RotationEnum.aimLeft, GetAngleFromVector(weaponDirection));
                            break;
                        case RotationEnum.aimUpLeft:
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUp", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimDown", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimRight", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimLeft", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpLeft", true);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpRight", false);
                            playerWeaponBehaviorComponent.Aim(RotationEnum.aimUpLeft, GetAngleFromVector(weaponDirection));
                            break;
                        case RotationEnum.aimUpRight:
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUp", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimDown", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimRight", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimLeft", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpLeft", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpRight", true);
                            playerWeaponBehaviorComponent.Aim(RotationEnum.aimUpRight, GetAngleFromVector(weaponDirection));
                            break;

                    }
                }         
                   
            }
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
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
