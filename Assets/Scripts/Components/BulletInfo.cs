using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct BulletInfo : IComponentData
{
    public float bulletSpeed;
    public int deliveryDamage;
    public bool isEnemyBullet;
   
}

public struct HealthOnBulletInfo: IComponentData
{
    public Entity healthPrefab;
}