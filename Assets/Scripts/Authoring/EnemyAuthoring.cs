using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Random = Unity.Mathematics.Random;

public class EnemyAuthoring : MonoBehaviour
{
    public int enemiesType;
    public float moveSpeed;
    public int currentHitPoint;
    public int maxHP;
    public int damage;
    public class Baker: Baker<EnemyAuthoring> 
    {
        public override void Bake(EnemyAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EnemiesInfo
            {
                enemiesType = authoring.enemiesType,
                moveSpeed = authoring.moveSpeed,
                maxHP = authoring.maxHP,
                currentHitPoint = authoring.currentHitPoint,
                damage = authoring.damage,
                random = Random.CreateFromIndex((uint)entity.GetHashCode()),
            }) ;
        }
    }
}
