using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class RocketAuthoring : MonoBehaviour
{
    public int BulletDamage;
    public float BulletSpeed;
    public float explosionRange;
    public GameObject healthPrefab;


    public class Baker : Baker<RocketAuthoring>
    {

        public override void Bake(RocketAuthoring authoring)
        {
            var Entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(Entity, new RocketInfo
            {
                bulletSpeed = authoring.BulletSpeed,
                deliveryDamage = authoring.BulletDamage,
                explosionRange = authoring.explosionRange
            });
            AddComponent(Entity, new BulletMovementInfo());
            AddComponent(Entity, new HealthOnBulletInfo
            {
                healthPrefab = GetEntity(authoring.healthPrefab, TransformUsageFlags.Dynamic)
            });
        }
    }
}
