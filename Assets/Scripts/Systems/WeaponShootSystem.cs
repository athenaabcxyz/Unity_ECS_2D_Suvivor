using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Random = Unity.Mathematics.Random;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using System;

[BurstCompile]
public partial struct WeaponShootSystem : ISystem
{
    private Random random; 
    private float nextShootICD;
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        random = Random.CreateFromIndex(1);
        state.RequireForUpdate<CurrentWeaponInfo>();
        
        nextShootICD = Time.time;
    }

    public void OnUpdate(ref SystemState state)
    {
        if(SystemAPI.TryGetSingletonEntity<PlayerInfoComponent>(out Entity entity))
        {
            var weaponInfo = state.EntityManager.GetComponentData<WeaponInfo>(entity);
            var playerInfo = state.EntityManager.GetComponentData<PlayerInfoComponent>(entity);
            var currentWeaponInfo = state.EntityManager.GetComponentData<CurrentWeaponInfo>(entity);
            var statMulti = state.EntityManager.GetComponentData<StateMultiplierInfo>(entity);

            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (SystemAPI.Time.ElapsedTime > nextShootICD)
                {
                    var bullet = state.EntityManager.Instantiate(weaponInfo.bulletPrefab);
                    state.EntityManager.SetComponentData(bullet, new LocalTransform
                    {
                        Position = currentWeaponInfo.weaponShootPosition,
                        Scale = playerInfo.bulletSize,
                        Rotation = Quaternion.identity,
                    });
                    state.EntityManager.SetComponentData(bullet, new BulletInfo
                    {
                        bulletSpeed = playerInfo.bulletSpeed,
                        deliveryDamage = Mathf.RoundToInt(playerInfo.deliveryDmg * statMulti.damageMultiplier),
                        isEnemyBullet = false
                    }) ;
                    state.EntityManager.SetComponentData(bullet, new BulletMovementInfo
                    {
                        
                        moveDirection = math.normalize(currentWeaponInfo.weaponShootDirection)+random.NextFloat3(new float3(-playerInfo.bulletSpread, -playerInfo.bulletSpread, 0), new float3(playerInfo.bulletSpread, playerInfo.bulletSpread, 0)),
                    });
                    nextShootICD = (float)SystemAPI.Time.ElapsedTime + weaponInfo.weaponShootICD;
                }
            }
        }
    }
}
