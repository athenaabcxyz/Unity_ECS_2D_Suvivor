using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct EnemyMovementInfo : IComponentData
{
    public float2 moveDirection;
    public float moveSpeed;
    public RotationEnum mouseAngle;
}
