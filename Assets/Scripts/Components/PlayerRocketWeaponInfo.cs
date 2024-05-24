using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct PlayerRocketWeaponInfo: IComponentData
{
    public Entity rocketPrefab;
    public float shootICD;
    public double shootCounter;
}