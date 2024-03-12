using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnComponentInfo_", menuName = "Scriptable Objects/Enemy/SpawnInfo")]
public class SpawnComponentSO : ScriptableObject
{
    public int quantity;
    public GameObject prefab;
}
