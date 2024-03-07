using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class EnemiesSpawnerAuthoring : MonoBehaviour
{
    public float BigDishMoveSpeed = 0.5f;
    public float BigFishMoveSpeedChase = 1f;

    public float MidDishMoveSpeed = 0.5f;
    public float MidFishMoveSpeedChase = 1f;

    public float DartDishMoveSpeed = 0.5f;
    public float DartFishMoveSpeedChase = 1f;

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
                moveSpeedUp = authoring.BigFishMoveSpeedChase
            });
            AddComponent(entity, new MidFishInfo
            {
                moveSpeed = authoring.MidDishMoveSpeed,
                moveSpeedUp = authoring.MidFishMoveSpeedChase
            });
            AddComponent(entity, new DartFishInfo
            {
                moveSpeed = authoring.DartDishMoveSpeed,
                moveSpeedUp = authoring.DartFishMoveSpeedChase
            });
        }
    }
}
