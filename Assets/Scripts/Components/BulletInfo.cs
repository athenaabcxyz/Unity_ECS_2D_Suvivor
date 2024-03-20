using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct BulletInfo : IComponentData
{
    public float bulletSpeed;
    public int deliveryDamage;
   
}

public struct HealthOnBulletInfo: IComponentData
{
    public Entity healthPrefab;
}