using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct CompanionStatMultiplierComponent : IComponentData
{
    public float damageMultiplier;
    public int healthIncresemet;
    public float speedMultiplier;
}
