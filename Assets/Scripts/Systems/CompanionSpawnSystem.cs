using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

[BurstCompile]
public partial struct CompanionSpawnSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerCompanionsInfo>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state) 
    {
        if(SystemAPI.TryGetSingletonEntity<PlayerCompanionsInfo>(out Entity player))
        {
            var companions = state.EntityManager.GetComponentData<PlayerCompanionsInfo>(player);
            var playerLocation = state.EntityManager.GetComponentData<LocalTransform>(player);
            if(Input.GetKeyDown(KeyCode.R))
            {
                var companion = state.EntityManager.Instantiate(companions.theScientist);
                state.EntityManager.SetComponentData<LocalTransform>(companion, new LocalTransform
                {
                    Position = new float3(playerLocation.Position.x - 2, playerLocation.Position.y, 0)
                });
                state.EntityManager.AddComponentData(companion, new RandomSeedComponent
                {
                    random = Random.CreateFromIndex((uint)companion.GetHashCode())
                });
                state.EntityManager.AddComponentData(companion, state.EntityManager.GetComponentData<WeaponInfo>(player));
                state.EntityManager.AddComponentData(companion, new CurrentWeaponInfo());
                state.EntityManager.AddComponentData(companion, new CurrentTarget { isAllowedToShoot = false, nextShootTime = SystemAPI.Time.ElapsedTime });
            }
            else
            {
                if(Input.GetKeyDown(KeyCode.T))
                {
                    var companion = state.EntityManager.Instantiate(companions.theThief);
                    state.EntityManager.SetComponentData(companion, new LocalTransform
                    {
                        Position = new float3(playerLocation.Position.x + 2, playerLocation.Position.y, 0)
                    });
                    state.EntityManager.AddComponentData(companion, new RandomSeedComponent
                    {
                        random = Random.CreateFromIndex((uint)companion.GetHashCode())
                    });
                    state.EntityManager.AddComponentData(companion, state.EntityManager.GetComponentData<WeaponInfo>(player));
                    state.EntityManager.AddComponentData(companion, new CurrentWeaponInfo());
                    state.EntityManager.AddComponentData(companion, new CurrentTarget { isAllowedToShoot = false, nextShootTime = SystemAPI.Time.ElapsedTime });
                }
            }
        }
    }

}
