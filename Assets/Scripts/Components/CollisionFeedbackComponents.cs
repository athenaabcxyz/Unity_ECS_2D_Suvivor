using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct PlayerAttackedFlag : IComponentData
{
    public int damage;
}
