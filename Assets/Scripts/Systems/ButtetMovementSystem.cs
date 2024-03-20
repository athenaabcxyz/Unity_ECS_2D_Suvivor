using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


[BurstCompile]
public partial struct BulletMovementSystem : ISystem
{

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerInfoComponent>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
       
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        var ecbParallelWritter = ecb.AsParallelWriter();
        new BulletMovementJob
        {
            deltaTime = SystemAPI.Time.DeltaTime,
            ecb = ecbParallelWritter,
        }.ScheduleParallel();
        state.Dependency.Complete();
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    [BurstCompile]
    public partial struct BulletMovementJob: IJobEntity
    { 
        public EntityCommandBuffer.ParallelWriter ecb;
        public float deltaTime;
        readonly void Execute([EntityIndexInQuery] int index, ref LocalTransform transform, ref BulletInfo info, Entity entity, in BulletMovementInfo moveInfo)
        {
            transform.Position += math.normalize(moveInfo.moveDirection)*info.bulletSpeed*deltaTime;

            if(transform.Position.x<-60||transform.Position.x>60|| transform.Position.y>35|| transform.Position.y<-35)
            {
                ecb.DestroyEntity(index, entity);
            }
        }
    }
}
