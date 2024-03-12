using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public struct EnemiesInfo: IComponentData
{
    public int enemiesType;
    public float moveSpeed;
    public float moveSpeedUp;
    public int currentHitPoint;
    public int damage;
    public int maxHitPoint;
    public Random random;
}