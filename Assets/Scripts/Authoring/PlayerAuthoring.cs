using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class PlayerAuthoring : MonoBehaviour
{
    Animator animator;
    [SerializeField] float speed;
    [SerializeField] int maxHitPoint;
    [SerializeField] float hitCoolDown = 1f;
    public GameObject buttletPrefab;
    public float weaponShotICD = 0.5f;
    public float bulletSize;
    public float bulletSpeed;
    public float bulletSpread;
    public int deliveryDmg;
    public float range;
    public GameObject theScientist;
    public GameObject theThief;

    // Start is called before the first frame update
    class Baker: Baker<PlayerAuthoring> 
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new WeaponInfo
            {
                bulletPrefab = GetEntity(authoring.buttletPrefab, TransformUsageFlags.Dynamic),
                weaponShootICD = authoring.weaponShotICD,
            });
            AddComponent(entity, new PlayerInfoComponent
            {
                Speed = authoring.speed,
                maxHitPoint = authoring.maxHitPoint,
                currentHitPoint = authoring.maxHitPoint,
                hitCoolDown = authoring.hitCoolDown,
                bulletSize = authoring.bulletSize,
                bulletSpeed = authoring.bulletSpeed,
                bulletSpread = authoring.bulletSpread,
                range = authoring.range,
                deliveryDmg = authoring.deliveryDmg,
            });
            AddComponent(entity, new PlayerCompanionsInfo
            {
                theScientist = GetEntity(authoring.theScientist, TransformUsageFlags.Dynamic),
                theThief = GetEntity(authoring.theThief, TransformUsageFlags.Dynamic),
            });
            AddComponent(entity, new LevelingInfoComponent
            {
                currentExp = 0,
                currentLevel = 1
            });
            AddComponent(entity, new StateMultiplierInfo
            {
                damageMultiplier = 1,
                speedMultiplier = 1,
                healthIncresemet = 0,
            });
        }
    } 
}

