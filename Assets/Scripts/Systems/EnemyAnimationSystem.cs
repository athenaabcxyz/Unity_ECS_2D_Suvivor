using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public partial struct EnemyAnimationSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<AnimationVisualsPoolList>();
        
    }

    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.ManagedAPI.TryGetSingleton(out AnimationVisualsPrefabs animationVisualsPrefabs))
        {
            return;
        }

        if (!SystemAPI.ManagedAPI.TryGetSingleton(out AnimationVisualsPoolList animationVisualsPoolList))
        {
            return;
        }


        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
        if(animationVisualsPoolList.VisualPools.Count <= 0)
        {
            for(int i = 0; i<10;i++)
            {
                animationVisualsPoolList.VisualPools.Add(new AnimationVisualsPool
                {
                    VisualPrefabPool = new List<GameObject>()
                });
            }
        }

        var enemiesQuery = SystemAPI.QueryBuilder().WithAll<EnemiesInfo>().Build();
        if (enemiesQuery.IsEmpty)
        {
            return;
        }
        else
        {
            foreach (var (transform, enemyInfo, entity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<EnemiesInfo>>().WithEntityAccess())
            {
                if (!state.EntityManager.HasComponent<VisualsReferenceComponent>(entity))
                {
                    GameObject enemyVisual;
                    if (animationVisualsPoolList.VisualPools[enemyInfo.ValueRO.enemiesType - 1].VisualPrefabPool.Count<=0)
                    {
                        enemyVisual = Object.Instantiate(animationVisualsPrefabs.VisualPrefab[enemyInfo.ValueRO.enemiesType]);
                    }
                    else
                    {
                        enemyVisual = animationVisualsPoolList.VisualPools[enemyInfo.ValueRO.enemiesType -1].VisualPrefabPool[animationVisualsPoolList.VisualPools[enemyInfo.ValueRO.enemiesType - 1].VisualPrefabPool.Count - 1].gameObject;
                        animationVisualsPoolList.VisualPools[enemyInfo.ValueRO.enemiesType - 1].VisualPrefabPool.Remove(enemyVisual);
                        if(enemyVisual.activeSelf == false)
                        {
                            enemyVisual.SetActive(true);
                        }
                    }

                    ecb.AddComponent(entity, new VisualsReferenceComponent { gameObject = enemyVisual });
                }
                else
                {
                    VisualsReferenceComponent enemyVisualRef = state.EntityManager.GetComponentData<VisualsReferenceComponent>(entity);
                    enemyVisualRef.gameObject.transform.position = transform.ValueRO.Position + new float3(0, -0.5f, 0);
                    if (state.EntityManager.HasComponent<EnemyMovementInfo>(entity))
                    {
                        EnemyMovementInfo enemyMovementInfo = state.EntityManager.GetComponentData<EnemyMovementInfo>(entity);
                        if (enemyMovementInfo.moveDirection.x != 0 || enemyMovementInfo.moveDirection.y != 0)
                        {
                            enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("isMoving", true);
                            enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("isIdle", false);

                        }
                        else
                        {
                            enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("isMoving", false);
                            enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("isIdle", true);
                        }
                        if(state.EntityManager.HasComponent<EnemiesShootInfo>(entity))
                        {
                            SystemAPI.TryGetSingletonEntity<PlayerInfoComponent>(out Entity player);
                            var playerWeaponBehaviorComponent = enemyVisualRef.gameObject.GetComponent<PlayerWeaponBehavior>();
                            var weaponShootPosition = (float3)playerWeaponBehaviorComponent.GetShootPosition();
                            var weaponPosition = (float3)playerWeaponBehaviorComponent.GetWeaponPosition();
                            var weaponDirection = state.EntityManager.GetComponentData<LocalTransform>(player).Position - (float3)weaponPosition;
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
                            playerWeaponBehaviorComponent.Aim(enemyMovementInfo.mouseAngle, GetAngleFromVector(weaponDirection));
                        }
                       
                        switch (enemyMovementInfo.mouseAngle)
                        {
                            case RotationEnum.aimUp:
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUp", true);
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimDown", false);
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimRight", false);
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimLeft", false);
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpLeft", false);
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpRight", false);
                               
                                break;
                            case RotationEnum.aimDown:
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUp", false);
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimDown", true);
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimRight", false);
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimLeft", false);
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpLeft", false);
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpRight", false);
                                break;
                            case RotationEnum.aimRight:
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUp", false);
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimDown", false);
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimRight", true);
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimLeft", false);
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpLeft", false);
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpRight", false);
                                break;
                            case RotationEnum.aimLeft:
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUp", false);
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimDown", false);
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimRight", false);
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimLeft", true);
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpLeft", false);
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpRight", false);
                                break;
                            case RotationEnum.aimUpLeft:
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUp", false);
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimDown", false);
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimRight", false);
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimLeft", false);
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpLeft", true);
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpRight", false);
                                break;
                            case RotationEnum.aimUpRight:
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUp", false);
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimDown", false);
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimRight", false);
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimLeft", false);
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpLeft", false);
                                enemyVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpRight", true);
                                break;

                        }
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
