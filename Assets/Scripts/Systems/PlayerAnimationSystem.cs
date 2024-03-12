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
                GameObject playerVisual = Object.Instantiate(animationVisualsPrefabs.Player);
                ecb.AddComponent(entity, new VisualsReferenceComponent { gameObject = playerVisual });
            }
            else
            {
                VisualsReferenceComponent playerVisualRef = state.EntityManager.GetComponentData<VisualsReferenceComponent>(entity);
                playerVisualRef.gameObject.transform.position = transform.ValueRO.Position;
                playerVisualRef.gameObject.transform.rotation = transform.ValueRO.Rotation;
                float2 staticPosition = new float2(0, 0);
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

                    switch (playerMovementInfo.mouseAngle)
                    {
                        case MouseRotationEnum.aimUp:
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUp", true);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimDown", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimRight", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimLeft", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpLeft", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpRight", false);
                            break;
                        case MouseRotationEnum.aimDown:
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUp", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimDown", true);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimRight", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimLeft", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpLeft", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpRight", false);
                            break;
                        case MouseRotationEnum.aimRight:
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUp", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimDown", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimRight", true);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimLeft", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpLeft", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpRight", false);
                            break;
                        case MouseRotationEnum.aimLeft:
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUp", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimDown", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimRight", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimLeft", true);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpLeft", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpRight", false);
                            break;
                        case MouseRotationEnum.aimUpLeft:
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUp", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimDown", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimRight", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimLeft", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpLeft", true);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpRight", false);
                            break;
                        case MouseRotationEnum.aimUpRight:
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUp", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimDown", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimRight", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimLeft", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpLeft", false);
                            playerVisualRef.gameObject.GetComponent<Animator>().SetBool("aimUpRight", true);
                            break;

                    }
                }

               
                if(state.EntityManager.HasComponent<PlayerAttackedFlag>(entity))
                {
                    playerVisualRef.gameObject.GetComponent<Animator>().Play("PlayerHurt");
                }
                   
            }
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
