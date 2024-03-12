using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public static class StaticEvents 
{ 
    /*public static SpawnComponentInfo GetSpawnComponentInfoBySO(SpawnComponentSO spawnComponentSO)
    {
        SpawnComponentInfo spawnComponentInfo = new SpawnComponentInfo
        {
            spawnQuantities = spawnComponentSO.quantity,
            enemyType = spawnComponentSO.enemyType,
        };
        return spawnComponentInfo;
    }

    public static NativeArray<SpawnComponentInfo> ConvertSOSpawnInfoToNativeArray(List<SpawnComponentSO> list)
    {
        NativeArray<SpawnComponentInfo> spawnList = new NativeArray<SpawnComponentInfo>(list.Count, Allocator.Temp);

        for(int i = 0; i < list.Count; i++)
        {
            spawnList[i] = GetSpawnComponentInfoBySO(list[i]);
        }

        return spawnList;
    }*/
}
