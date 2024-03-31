using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public partial struct PlayerLevelUpSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<LevelingInfoComponent>();
    }

    public void OnUpdate(ref SystemState state)
    {
        if(SystemAPI.TryGetSingletonEntity<PlayerInfoComponent>(out Entity player))
        {          
            var levelingInfo = state.EntityManager.GetComponentData<LevelingInfoComponent>(player);
            var playerInfo = state.EntityManager.GetComponentData<PlayerInfoComponent>(player);
            var stat = state.EntityManager.GetComponentData<StateMultiplierInfo>(player);
            if (levelingInfo.currentExp>= levelingInfo.currentLevel*10)
            {
                levelingInfo.currentExp -= levelingInfo.currentLevel * 10;
                levelingInfo.currentLevel++;                     
                playerInfo.currentHitPoint = playerInfo.maxHitPoint + stat.healthIncresemet;
                state.EntityManager.SetComponentData(player, levelingInfo);
                state.EntityManager.SetComponentData(player, playerInfo);
            }
        }
    }
}
