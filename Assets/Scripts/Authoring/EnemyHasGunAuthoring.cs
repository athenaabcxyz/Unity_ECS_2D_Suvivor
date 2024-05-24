using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Random = Unity.Mathematics.Random;

public class EnemyHasGunAuthoring : MonoBehaviour
{
    public int enemiesType;
    public float moveSpeed;
    public int currentHitPoint;
    public int maxHP;
    public int damage;
    public float ICD;
    public float bulletSpeed;
    public GameObject bulletPrefab;
    public class Baker : Baker<EnemyHasGunAuthoring>
    {
        public override void Bake(EnemyHasGunAuthoring authoring)
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
            });
            AddComponent(entity, new EnemiesShootInfo
            {
                damage = authoring.damage,
                bulletPrefab = GetEntity(authoring.bulletPrefab, TransformUsageFlags.Dynamic),
                bulletSpeed = authoring.bulletSpeed,
                weaponShootICD = authoring.ICD,
                shootCounter = 0
            });
        }
    }
}