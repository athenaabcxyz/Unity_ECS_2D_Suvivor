using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using UnityEngine.AI;

public class AnimationVisualAuthoring : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject BigFish;
    [SerializeField] private GameObject DartFish;
    [SerializeField] private GameObject MidFish;

    private class Baker: Baker<AnimationVisualAuthoring> 
    {
        public override void Bake(AnimationVisualAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponentObject(entity, new AnimationVisualsPrefabs
            {
                Player = authoring.playerPrefab,
                BigFish = authoring.BigFish,
                DartFish = authoring.DartFish,
                MidFish = authoring.MidFish,
            });
        }
    }
}
