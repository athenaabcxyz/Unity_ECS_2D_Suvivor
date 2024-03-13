using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct EnemyAnimationSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.ManagedAPI.TryGetSingleton(out AnimationVisualsPrefabs animationVisualsPrefabs))
        {
            return;
        }

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        var enemiesQuery = SystemAPI.QueryBuilder().WithAll<EnemiesInfo>().Build();
        if (enemiesQuery.IsEmpty)
        { 

        }
        else
        {
            foreach (var (transform, enemyInfo, entity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<EnemiesInfo>>().WithEntityAccess())
            {
                if (!state.EntityManager.HasComponent<VisualsReferenceComponent>(entity))
                {

                    GameObject enemyVisual = Object.Instantiate(animationVisualsPrefabs.VisualPrefab[enemyInfo.ValueRO.enemiesType]);

                    ecb.AddComponent(entity, new VisualsReferenceComponent { gameObject = enemyVisual });
                }
                else
                {
                    VisualsReferenceComponent enemyVisualRef = state.EntityManager.GetComponentData<VisualsReferenceComponent>(entity);
                    enemyVisualRef.gameObject.transform.position = transform.ValueRO.Position + new float3(0, -0.5f, 0);
                    enemyVisualRef.gameObject.transform.rotation = transform.ValueRO.Rotation;
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
}
