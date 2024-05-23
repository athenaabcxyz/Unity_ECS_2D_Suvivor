using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct RocketInfo : IComponentData
{
    public float bulletSpeed;
    public int deliveryDamage;
    public float explosionRange;
}

