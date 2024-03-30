using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class BulletAuthoring : MonoBehaviour
{
    public int BulletDamage;
    public float BulletSpeed;
    public GameObject healthPrefab;

    
    public class Baker : Baker<BulletAuthoring>
    {
       
        public override void Bake(BulletAuthoring authoring)
        {
            var Entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(Entity, new BulletInfo
            {
                bulletSpeed = authoring.BulletSpeed,
                deliveryDamage = authoring.BulletDamage,               
            });
            AddComponent(Entity, new BulletMovementInfo());
            AddComponent(Entity, new HealthOnBulletInfo
            {
                healthPrefab = GetEntity(authoring.healthPrefab, TransformUsageFlags.Dynamic)
            });
        }
    }
}
