using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct LevelingInfoComponent : IComponentData
{
    public int currentLevel;
    public long currentExp;
}
