using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using UnityEngine.AI;
using Unity.VisualScripting;

public class AnimationVisualAuthoring : MonoBehaviour
{
    [SerializeField] private List<GameObject> VisualPrefabs;

    public List<AnimationVisualsPool> VisualPools = new List<AnimationVisualsPool>(10);

    private class Baker: Baker<AnimationVisualAuthoring> 
    {
        
        public override void Bake(AnimationVisualAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponentObject(entity, new AnimationVisualsPoolList
            {
                elementQuantity = 10,
                VisualPools = authoring.VisualPools,
            });
            AddComponentObject(entity, new AnimationVisualsPrefabs
            {
               
                VisualPrefab = authoring.VisualPrefabs,
            });
            
        }
    }
}
