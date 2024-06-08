using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct CurrentWeaponInfo : IComponentData
{
    public float3 weaponShootPosition;
    public float3 weaponShootDirection;
}
public struct WeaponInfo : IComponentData
{
    public Entity bulletPrefab;
    public float weaponShootICD;

}

public struct CurrentTarget: IComponentData
{
    public float3 currentTarget;
    public bool isAllowedToShoot;
    public double nextShootTime;
}
