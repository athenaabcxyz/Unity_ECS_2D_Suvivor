using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class HealthAuthoring : MonoBehaviour
{
    public float healEfficiency;
    public class Baker : Baker<HealthAuthoring>
    {
        public override void Bake(HealthAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new HealthComponent
            {
                healPercentage = authoring.healEfficiency,
            });
        }
    }

}

