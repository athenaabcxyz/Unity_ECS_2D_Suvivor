using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Random = Unity.Mathematics.Random;

public class EnemyAuthoring : MonoBehaviour
{
    public EnemyDetailSO enemy;

    public class Baker: Baker<EnemyAuthoring> 
    {
        public override void Bake(EnemyAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EnemiesInfo
            {
                enemiesType = authoring.enemy.enemiesType,
                moveSpeed = authoring.enemy.moveSpeed,
                moveSpeedUp = authoring.enemy.moveSpeedUp,
                currentHitPoint = authoring.enemy.currentHitPoint,
                damage = authoring.enemy.damage,
                maxHitPoint = authoring.enemy.maxHitPoint,
                random = Random.CreateFromIndex((uint)entity.GetHashCode()),
            });
        }
    }
}
