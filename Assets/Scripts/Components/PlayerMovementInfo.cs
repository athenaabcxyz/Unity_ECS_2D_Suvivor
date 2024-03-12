using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

public struct PlayerMovementInfo : IComponentData
{
    public float2 moveDirection;
    public float moveSpeed;
    public MouseRotationEnum mouseAngle;
    public float3 mousePosition;
}

public enum MouseRotationEnum
{
    aimUp, aimDown, aimLeft, aimRight, aimUpLeft, aimUpRight 
}
