using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class EnemiesSpawnerAuthoring : MonoBehaviour
{
    public float BigDishMoveSpeed = 0.5f;
    public float BigFishMoveSpeedChase = 1f;
    public int maxHitPointBigFish = 15;
    public int damageBigFish = 1;

    public float MidDishMoveSpeed = 0.5f;
    public float MidFishMoveSpeedChase = 1f;
    public int maxHitPointMidFish = 10;
    public int damageMidFish = 2;

    public float DartDishMoveSpeed = 1f;
    public float DartFishMoveSpeedChase = 1.5f;
    public int maxHitPointDartFish = 5;
    public int damageDartFish = 1;

    public int BigFishQuantity;
    public int MidFishQuantity;
    public int DartFishQuantity;

    [SerializeField] GameObject enemyPrefab;

    public class Baker : Baker<EnemiesSpawnerAuthoring>
    {
        public override void Bake(EnemiesSpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new SpawnerInfo
            {
                BigFishQuantity = authoring.BigFishQuantity,
                MidFishQuantity = authoring.MidFishQuantity,
                DartFishQuantity = authoring.DartFishQuantity,
                enemy = GetEntity(authoring.enemyPrefab, TransformUsageFlags.Dynamic),
            });
            AddComponent(entity, new BigFishInfo
            {
                moveSpeed = authoring.BigDishMoveSpeed,
                moveSpeedUp = authoring.BigFishMoveSpeedChase,
                maxHitPoint = authoring.maxHitPointBigFish,
                damage = authoring.damageBigFish
            });
            AddComponent(entity, new MidFishInfo
            {
                moveSpeed = authoring.MidDishMoveSpeed,
                moveSpeedUp = authoring.MidFishMoveSpeedChase,
                maxHitPoint = authoring.maxHitPointMidFish,
                damage = authoring.damageMidFish
            });
            AddComponent(entity, new DartFishInfo
            {
                moveSpeed = authoring.DartDishMoveSpeed,
                moveSpeedUp = authoring.DartFishMoveSpeedChase,
                maxHitPoint = authoring.maxHitPointDartFish,
                damage = authoring.damageDartFish
            });
        }
    }
}
