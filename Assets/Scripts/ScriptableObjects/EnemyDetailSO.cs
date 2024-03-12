using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EnemyDetail_", menuName = "Scriptable Objects/Enemy/EnemyDetail")]
public class EnemyDetailSO : ScriptableObject
{
    public int enemiesType;
    public float moveSpeed;
    public float moveSpeedUp;
    public int currentHitPoint;
    public int damage;
    public int maxHitPoint;
}
