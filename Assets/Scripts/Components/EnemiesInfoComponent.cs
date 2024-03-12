using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;


public struct EnemiesInfo: IComponentData
{
    public int enemiesType;
    public float moveSpeed;
    public int maxHP;
    public int currentHitPoint;
    public int damage;
    public Random random;
}