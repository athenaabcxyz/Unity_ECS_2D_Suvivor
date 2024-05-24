using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct BulletHitFlag : IComponentData
{
    public int damage;
    public Entity healthPrefab;
}
