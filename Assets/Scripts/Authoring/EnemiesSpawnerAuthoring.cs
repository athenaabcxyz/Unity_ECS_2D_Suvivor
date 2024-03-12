using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class EnemiesSpawnerAuthoring : MonoBehaviour
{

    public int Grimonk_BrownSpawnQuatity;
    public GameObject Grimonk_Brown;

    public int Hedusa_GreenSpawnQuatity;
    public GameObject Hedusa_Green;

    public int MudRock_BrownSpawnQuatity;
    public GameObject MudRock_Brown;


    public class Baker : Baker<EnemiesSpawnerAuthoring>
    {
        public override void Bake(EnemiesSpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new SpawnerInfo
            {
                Grimonk_BrownSpawnQuatity = authoring.Grimonk_BrownSpawnQuatity,
                Hedusa_GreenSpawnQuatity = authoring.Hedusa_GreenSpawnQuatity,
                MudRock_BrownSpawnQuatity = authoring.MudRock_BrownSpawnQuatity,
                Grimonk_Brown = GetEntity(authoring.Grimonk_Brown, TransformUsageFlags.Dynamic),
                Hedusa_Green = GetEntity(authoring.Hedusa_Green, TransformUsageFlags.Dynamic),
                MudRock_Brown = GetEntity(authoring.MudRock_Brown, TransformUsageFlags.Dynamic),
            });           
        }
    }
}
