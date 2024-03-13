using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class EnemiesSpawnerAuthoring : MonoBehaviour
{

    public int Grimonk_BrownSpawnQuatity;
    public GameObject Grimonk_Brown;

    public int Hedusa_BlueSpawnQuatity;
    public GameObject Hedusa_Blue;

    public int Hedusa_GreenSpawnQuatity;
    public GameObject Hedusa_Green;

    public int Hedusa_RedSpawnQuatity;
    public GameObject Hedusa_Red;

    public int MudRock_BrownSpawnQuatity;
    public GameObject MudRock_Brown;

    public int Orc_FleshSpawnQuatity;
    public GameObject Orc_Flesh;

    public int SlimeBlock_BlueSpawnQuatity;
    public GameObject SlimeBlock_Blue;

    public int SlimeBlock_GreenSpawnQuatity;
    public GameObject SlimeBlock_Green;

    public int SlimeBlock_RedSpawnQuatity;
    public GameObject SlimeBlock_Red;

    public int SlizzardSpawnQuatity;
    public GameObject Slizzard;


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
                Hedusa_BlueSpawnQuatity = authoring.Hedusa_BlueSpawnQuatity,
                Hedusa_RedSpawnQuatity = authoring.Hedusa_RedSpawnQuatity,
                Orc_FleshSpawnQuatity = authoring.Orc_FleshSpawnQuatity,
                SlimeBlock_BlueSpawnQuatity = authoring.SlimeBlock_BlueSpawnQuatity,
                SlimeBlock_GreenSpawnQuatity = authoring.SlimeBlock_GreenSpawnQuatity,
                SlimeBlock_RedSpawnQuatity = authoring.SlimeBlock_RedSpawnQuatity,
                SlizzardSpawnQuatity = authoring.SlizzardSpawnQuatity,

                Grimonk_Brown = GetEntity(authoring.Grimonk_Brown, TransformUsageFlags.Dynamic),
                Hedusa_Green = GetEntity(authoring.Hedusa_Green, TransformUsageFlags.Dynamic),
                MudRock_Brown = GetEntity(authoring.MudRock_Brown, TransformUsageFlags.Dynamic),
                Hedusa_Blue = GetEntity(authoring.Hedusa_Blue, TransformUsageFlags.Dynamic),
                Hedusa_Red = GetEntity(authoring.Hedusa_Red, TransformUsageFlags.Dynamic),
                Orc_Flesh = GetEntity(authoring.Orc_Flesh, TransformUsageFlags.Dynamic),
                SlimeBlock_Red = GetEntity(authoring.SlimeBlock_Red, TransformUsageFlags.Dynamic),
                SlimeBlock_Blue = GetEntity(authoring.SlimeBlock_Blue, TransformUsageFlags.Dynamic),
                SlimeBlock_Green = GetEntity(authoring.SlimeBlock_Green, TransformUsageFlags.Dynamic),
                Slizzard = GetEntity(authoring.Slizzard, TransformUsageFlags.Dynamic),
            });         
        }
    }
}
