using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraControllerGO : MonoBehaviour
{
    public GameObject player;
    public float maxXSize = 42f;
    public float minXSize = -42f;
    public float maxYSize = 25;
    public float minYSize = -25f;

    void LateUpdate()
    {
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        if (player.transform.position.x < maxXSize && player.transform.position.x > minXSize)
        {
            newPosition.x = player.transform.position.x;
        }
        if (player.transform.position.y < maxYSize && player.transform.position.y > minYSize)
        {
            newPosition.y = player.transform.position.y;
        }

        transform.position = newPosition;
    }
}
