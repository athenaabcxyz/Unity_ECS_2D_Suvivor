using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Random = Unity.Mathematics.Random;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

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

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if(SystemAPI.TryGetSingletonEntity<PlayerInfoComponent>(out Entity entity))
        {
            var weaponInfo = state.EntityManager.GetComponentData<WeaponInfo>(entity);
            var currentWeaponInfo = state.EntityManager.GetComponentData<CurrentWeaponInfo>(entity);

            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (SystemAPI.Time.ElapsedTime > nextShootICD)
                {
                    var bullet = state.EntityManager.Instantiate(weaponInfo.bulletPrefab);
                    state.EntityManager.SetComponentData(bullet, new LocalTransform
                    {
                        Position = currentWeaponInfo.weaponShootPosition,
                        Scale = 1.5f,
                        Rotation = Quaternion.identity,
                    });
                    state.EntityManager.SetComponentData(bullet, new BulletMovementInfo
                    {
                        moveDirection = math.normalize(currentWeaponInfo.weaponShootDirection)+random.NextFloat3(new float3(-0.05f, -0.05f,0), new float3(0.05f, 0.05f, 0)),
                    });
                    nextShootICD = (float)SystemAPI.Time.ElapsedTime + weaponInfo.weaponShootICD;
                }
            }
        }
    }
}
