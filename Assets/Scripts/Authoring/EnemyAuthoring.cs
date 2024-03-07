using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Random = Unity.Mathematics.Random;

public class EnemyAuthoring : MonoBehaviour
{
    public class Baker: Baker<EnemyAuthoring> 
    {
        public override void Bake(EnemyAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EnemiesInfo
            {
                random = Random.CreateFromIndex((uint)entity.GetHashCode()),
            });
        }
    }
}
