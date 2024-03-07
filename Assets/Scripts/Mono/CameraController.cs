using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour {

    private EntityManager entityManager;
    [SerializeField] GameObject staticBackground;
    private void Awake()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }
    void LateUpdate () {
            
        if (entityManager != null)
        {
            var query = entityManager.CreateEntityQuery(ComponentType.ReadWrite<LocalTransform>(), ComponentType.ReadOnly<PlayerInfoComponent>());
            if (!query.IsEmpty)
            {
                foreach (var entity in query.ToEntityArray(Allocator.Temp))
                {
                    var position = entityManager.GetComponentData<LocalTransform>(entity).Position;
                    transform.position = new Vector3(position.x, position.y, -10);
                    staticBackground.transform.position = new Vector3(position.x, position.y, 1);
                }
            }
        }
	}
}
