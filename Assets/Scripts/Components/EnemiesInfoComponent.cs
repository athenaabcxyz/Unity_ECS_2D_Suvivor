using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public struct DartFishInfo : IComponentData
{
    public float moveSpeed;
    public float moveSpeedUp;
    public int maxHitPoint;
    public int damage;
}


public struct BigFishInfo : IComponentData
{
    public float moveSpeed;
    public float moveSpeedUp;
    public int maxHitPoint;
    public int damage;
}
public struct MidFishInfo : IComponentData
{
    public float moveSpeed;
    public float moveSpeedUp;
    public int maxHitPoint;
    public int damage;
}

public struct EnemiesInfo: IComponentData
{
    public int enemiesType;
    public float moveSpeed;
    public float moveSpeedUp;
    public int currentHitPoint;
    public int damage;
    public Random random;
}