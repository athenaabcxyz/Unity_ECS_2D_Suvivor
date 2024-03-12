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

    public float maxXSize = 42f;
    public float minXSize = -42f;
    public float maxYSize = 25;
    public float minYSize = -25f;
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
                    Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    if (position.x < maxXSize && position.x > minXSize )
                    {
                        newPosition.x = position.x;
                    }
                    if(position.y < maxYSize && position.y > minYSize)
                    {
                        newPosition.y = position.y;
                    }

                    transform.position = newPosition;

                }
            }
        }
	}
}
