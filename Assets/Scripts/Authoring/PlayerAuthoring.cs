using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class PlayerAuthoring : MonoBehaviour
{
    Animator animator;
    [SerializeField] float speed;
    [SerializeField] float speedAccelerate;
    [SerializeField] float speedDecelerate;

    // Start is called before the first frame update
    class Baker: Baker<PlayerAuthoring> 
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PlayerInfoComponent
            {
                Speed = authoring.speed,
                SpeedAccelerate = authoring.speedAccelerate,
                SpeedDecelerator = authoring.speedDecelerate,
            });
        }
    } 
}

