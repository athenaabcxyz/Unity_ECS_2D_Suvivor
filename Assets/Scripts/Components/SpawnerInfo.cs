using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct SpawnerInfo : IComponentData
{
    public int BigFishQuantity;
    public int MidFishQuantity;
    public int DartFishQuantity;
    public Entity enemy;
}
