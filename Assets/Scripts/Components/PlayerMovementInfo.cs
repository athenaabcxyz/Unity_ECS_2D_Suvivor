using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

public struct PlayerMovementInfo : IComponentData
{
    public float2 moveDirection;
    public float moveSpeed;
    public RotationEnum mouseAngle;
    public float3 mousePosition;
    public float3 weaponShootPosition;
}
