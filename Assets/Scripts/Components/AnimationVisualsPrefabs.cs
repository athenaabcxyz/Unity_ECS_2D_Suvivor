using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;


public class AnimationVisualsPrefabs : IComponentData
{
    public List<GameObject> VisualPrefab;
}


public class AnimationVisualsPoolList : IComponentData
{
    public int elementQuantity;
    public List<AnimationVisualsPool> VisualPools;
}

public class AnimationVisualsPool: IComponentData
{
    public List<GameObject> VisualPrefabPool;
}