using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct SpawnerInfo : IComponentData
{
    public int Grimonk_BrownSpawnQuatity;
    public int Hedusa_GreenSpawnQuatity;
    public int MudRock_BrownSpawnQuatity;
    public Entity Grimonk_Brown;
    public Entity Hedusa_Green;
    public Entity MudRock_Brown;
}
