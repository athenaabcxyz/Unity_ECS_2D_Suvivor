using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;
using UnityEngine;

public struct CompanionInfo : IComponentData
{
    public float Speed;
    public int maxHitPoint;
    public int currentHitPoint;
    public float hitCoolDown;
    public int deliveryDmg;
    public float attackICD;
    public int companionType;
    public float bulletSize;
    public float bulletSpeed;
    public float bulletSpread;
    public float range;
}
public struct RandomSeedComponent: IComponentData
{
    public Random random;
}

public struct CompanionMovementInfo: IComponentData
{
    public float3 moveDirection;
    public RotationEnum aimDirection;
}