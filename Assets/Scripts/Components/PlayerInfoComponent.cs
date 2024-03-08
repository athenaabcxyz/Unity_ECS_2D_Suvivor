using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct PlayerInfoComponent : IComponentData
{
    public float Speed;
    public float SpeedAccelerate;
    public float SpeedDecelerator;
    public int maxHitPoint;
    public int currentHitPoint;
    public float hitCoolDown;
}

