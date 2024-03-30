using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct CompanionAttackSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<CompanionMovementInfo>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);
        var handler = new CompanionAttackJob
        {
            elaspedTime = SystemAPI.Time.ElapsedTime,
            deltaTime = SystemAPI.Time.DeltaTime,
            ecb = ecb.AsParallelWriter()
        }.ScheduleParallel(new Unity.Jobs.JobHandle());
        handler.Complete();
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    
    [BurstCompile]
    public partial struct CompanionAttackJob : IJobEntity
    {
        public float deltaTime;
        public double elaspedTime;
        public EntityCommandBuffer.ParallelWriter ecb;
        public void Execute([EntityIndexInQuery] int index, ref RandomSeedComponent random, ref LocalTransform transform, ref CurrentTarget target, ref CompanionInfo companion, ref CompanionMovementInfo movementInfo, Entity entity, ref WeaponInfo weapon, ref CurrentWeaponInfo currentWeapon)
        {
            if (target.isAllowedToShoot == true)
            {
                if (elaspedTime > target.nextShootTime)
                {
                    var bullet = ecb.Instantiate(index, weapon.bulletPrefab);
                    ecb.SetComponent(index, bullet, new LocalTransform
                    {
                        Position = currentWeapon.weaponShootPosition,
                        Scale = companion.bulletSize,
                        Rotation = Quaternion.identity,
                    });
                    ecb.SetComponent(index, bullet, new BulletInfo
                    {
                        bulletSpeed = companion.bulletSpeed,
                        deliveryDamage = companion.deliveryDmg,
                    });
                    ecb.SetComponent(index, bullet, new BulletMovementInfo
                    {
                        moveDirection = math.normalize(currentWeapon.weaponShootDirection)+random.random.NextFloat3(new float3(-companion.bulletSpread, -companion.bulletSpread, 0), new float3(companion.bulletSpread, companion.bulletSpread, 0)),
                    });
                    target.nextShootTime = elaspedTime + companion.attackICD;
                }
            }

        }

    }
}
