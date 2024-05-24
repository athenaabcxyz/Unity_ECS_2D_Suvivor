using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;

public partial struct EnemyShootSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EnemiesShootInfo>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if(SystemAPI.TryGetSingletonEntity<PlayerInfoComponent>(out var player))
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);
            var handler = new EnemyShootJob
            {
                elaspedTime = SystemAPI.Time.ElapsedTime,
                deltaTime = SystemAPI.Time.DeltaTime,
                ecb = ecb.AsParallelWriter(),
                playerPosition = state.EntityManager.GetComponentData<LocalTransform>(player).Position,

            }.ScheduleParallel(new Unity.Jobs.JobHandle());
            handler.Complete();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    
    }

    public partial struct EnemyShootJob: IJobEntity
    {
        public float deltaTime;
        public double elaspedTime;
        public EntityCommandBuffer.ParallelWriter ecb;
        public float3 playerPosition;
        public void Execute([EntityIndexInQuery] int index, ref LocalTransform transform, ref EnemiesShootInfo shootInfo, ref EnemiesInfo info, ref CurrentWeaponInfo weapon)
        {
            if (elaspedTime > shootInfo.shootCounter && math.distance(playerPosition, transform.Position)<=8f)
            {
                var bullet = ecb.Instantiate(index, shootInfo.bulletPrefab);
                ecb.SetComponent(index, bullet, new LocalTransform
                {
                    Position = weapon.weaponShootPosition,
                    Scale = 1,
                    Rotation = Quaternion.identity,
                });
                ecb.SetComponent(index, bullet, new BulletInfo
                {
                    bulletSpeed = shootInfo.bulletSpeed,
                    deliveryDamage = shootInfo.damage,
                    isEnemyBullet = true
                });
                ecb.SetComponent(index, bullet, new BulletMovementInfo
                {
                    moveDirection = weapon.weaponShootDirection,
                });
                shootInfo.shootCounter = elaspedTime + shootInfo.weaponShootICD;
            }
        }
    }
}
