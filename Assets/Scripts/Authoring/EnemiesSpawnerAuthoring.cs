using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using System;

public class EnemiesSpawnerAuthoring : MonoBehaviour
{
    public List<SpawnComponentSO> spawnComponents = new List<SpawnComponentSO>();

    public class Baker : Baker<EnemiesSpawnerAuthoring>
    {

        public override void Bake(EnemiesSpawnerAuthoring authoring)
        {
            NativeArray<SpawnComponentInfo> nativeList = new NativeArray<SpawnComponentInfo>(authoring.spawnComponents.Count, Allocator.Temp);

            for (int i = 0; i < authoring.spawnComponents.Count; i++)
            {
                nativeList[i] = new SpawnComponentInfo
                {
                    spawnQuantities = authoring.spawnComponents[i].quantity,
                    enemiesPrefab = GetEntity(authoring.spawnComponents[i].prefab, TransformUsageFlags.Dynamic),
                };
            }
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new SpawnerInfo
            {
                spawnInfo = nativeList,
            });
        }
    }
}
