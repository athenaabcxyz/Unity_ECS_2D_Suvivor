using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;


public class CompanionAuthoring : MonoBehaviour
{
    public float Speed;
    public int maxHitPoint;
    public int currentHitPoint;
    public float hitCoolDown;
    public int deliveryDmg;
    public float attackICD;
    public int companionType;
    public float bulletSize;
    public float bulletSpeed;
    public float bulletSpread;
    public float range;
    public class Baker : Baker<CompanionAuthoring>
    {
        public override void Bake(CompanionAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CompanionInfo
            {
                Speed = authoring.Speed,
                maxHitPoint = authoring.maxHitPoint,
                currentHitPoint = authoring.currentHitPoint,
                hitCoolDown = authoring.hitCoolDown,
                deliveryDmg = authoring.deliveryDmg,
                attackICD = authoring.attackICD,
                companionType = authoring.companionType,
                bulletSize = authoring.bulletSize,
                bulletSpeed = authoring.bulletSpeed,
                bulletSpread = authoring.bulletSpread,
                range = authoring.range
            });
            AddComponent(entity, new CompanionMovementInfo());
        }
    }
}
