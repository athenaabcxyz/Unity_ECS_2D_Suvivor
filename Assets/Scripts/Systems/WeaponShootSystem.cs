using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct WeaponShootSystem : ISystem
{
    private float nextShootICD;
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<CurrentWeaponInfo>();
        nextShootICD = Time.time;
    }

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
                        Scale = 1f,
                        Rotation = Quaternion.identity,
                    });
                    state.EntityManager.SetComponentData(bullet, new BulletMovementInfo
                    {
                        moveDirection = currentWeaponInfo.weaponShootDirection,
                    });
                    nextShootICD = (float)SystemAPI.Time.ElapsedTime + weaponInfo.weaponShootICD;
                }
            }
        }
    }
}
