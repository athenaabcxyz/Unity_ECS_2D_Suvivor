using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct PlayerInfoComponent : IComponentData
{
    public float Speed;
    public int maxHitPoint;
    public int currentHitPoint;
    public float hitCoolDown;
    public float bulletSize;
    public float bulletSpeed;
    public float bulletSpread;
    public int deliveryDmg;
    public float range;
}

public struct PlayerCompanionsInfo: IComponentData
{
    public Entity theScientist;
    public Entity theThief;
}


