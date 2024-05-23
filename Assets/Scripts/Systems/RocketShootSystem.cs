using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct RocketShootSystem: ISystem
{
    private float nextShootICD;
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerRocketWeaponInfo>();
        state.RequireForUpdate<CurrentWeaponInfo>();
        nextShootICD = Time.time;
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (SystemAPI.TryGetSingletonEntity<PlayerInfoComponent>(out Entity entity))
        {
            var rocketInfo = state.EntityManager.GetComponentData<PlayerRocketWeaponInfo>(entity);
            var playerInfo = state.EntityManager.GetComponentData<PlayerInfoComponent>(entity);
            var currentWeaponInfo = state.EntityManager.GetComponentData<CurrentWeaponInfo>(entity);
            var statMulti = state.EntityManager.GetComponentData<StateMultiplierInfo>(entity);

            if (Input.GetKey(KeyCode.Mouse1))
            {
                if (SystemAPI.Time.ElapsedTime > nextShootICD)
                {
                    var bullet = state.EntityManager.Instantiate(rocketInfo.rocketPrefab);
                    state.EntityManager.SetComponentData(bullet, new LocalTransform
                    {
                        Position = currentWeaponInfo.weaponShootPosition,
                        Scale = playerInfo.bulletSize,
                        Rotation = Quaternion.identity,
                    });
                    state.EntityManager.SetComponentData(bullet, new BulletMovementInfo
                    {

                        moveDirection = math.normalize(currentWeaponInfo.weaponShootDirection),
                    });
                    nextShootICD = (float)SystemAPI.Time.ElapsedTime + rocketInfo.shootICD;
                }
            }
        }
    }
}