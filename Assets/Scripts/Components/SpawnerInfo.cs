using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public struct SpawnerInfo : IComponentData
{
    public NativeArray<SpawnComponentInfo> spawnInfo;
}

public struct SpawnComponentInfo: IComponentData
{
    public int spawnQuantities;
    public Entity enemiesPrefab;
}
