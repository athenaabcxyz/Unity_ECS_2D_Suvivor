using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using UnityEngine.AI;

public class AnimationVisualAuthoring : MonoBehaviour
{
   public List<AnimationVisualPrefabsSO> animationVisualPrefabsObjects = new List<AnimationVisualPrefabsSO>();

    private class Baker : Baker<AnimationVisualAuthoring>
    {
        public override void Bake(AnimationVisualAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponentObject(entity, new AnimationVisualsPrefabs
            {
                list = authoring.animationVisualPrefabsObjects,
            });
        }
    }
}
