using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public struct DartFishInfo : IComponentData
{
    public float moveSpeed;
    public float moveSpeedUp;

}


public struct BigFishInfo : IComponentData
{
    public float moveSpeed;
    public float moveSpeedUp;
}
public struct MidFishInfo : IComponentData
{
    public float moveSpeed;
    public float moveSpeedUp;
}

public struct EnemiesInfo: IComponentData
{
    public int enemiesType;
    public float moveSpeed;
    public float moveSpeedUp;
    public Random random;
}