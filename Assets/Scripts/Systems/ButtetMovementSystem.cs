using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct BulletMovementSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerInfoComponent>();
    }

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
