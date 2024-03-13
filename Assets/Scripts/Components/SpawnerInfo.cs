using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct SpawnerInfo : IComponentData
{
    public int Grimonk_BrownSpawnQuatity;
    public int Hedusa_BlueSpawnQuatity;
    public int Hedusa_GreenSpawnQuatity;
    public int Hedusa_RedSpawnQuatity;
    public int MudRock_BrownSpawnQuatity;
    public int Orc_FleshSpawnQuatity;
    public int SlimeBlock_BlueSpawnQuatity;
    public int SlimeBlock_GreenSpawnQuatity;
    public int SlimeBlock_RedSpawnQuatity;
    public int SlizzardSpawnQuatity;
    public Entity Grimonk_Brown;
    public Entity Hedusa_Blue;
    public Entity Hedusa_Green;
    public Entity Hedusa_Red;
    public Entity MudRock_Brown;
    public Entity Orc_Flesh;
    public Entity SlimeBlock_Blue;
    public Entity SlimeBlock_Green;
    public Entity SlimeBlock_Red;
    public Entity Slizzard;
}
