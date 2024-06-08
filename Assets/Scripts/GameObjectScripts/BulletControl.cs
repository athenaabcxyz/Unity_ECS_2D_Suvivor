using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using System;
using Unity.Entities;
using Unity.Transforms;

public class BulletControl : MonoBehaviour
{
    public int BulletDamage;
    public float BulletSpeed;
    public GameObject healthPrefab;
    public GameObject spawner;
    private Vector3 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveBullet();
        BulletDamageCall();
    }

    private void MoveBullet()
    {
        transform.position += (Vector3)math.normalize(moveDirection) * BulletSpeed * Time.deltaTime;
        if (transform.position.x < -60 || transform.position.x > 60 || transform.position.y > 35 || transform.position.y < -35)
        {
            Destroy(this.gameObject);
        }
    }

    public void BulletDamageCall()
    {

        GameObject closetEnemy = null;
        float smalestDistance = 0.8f;
        bool isCollided = false;
        foreach (var enemy in spawner.GetComponent<EnemiesSpawner>().EnemyList)
        {
            if (math.distance(transform.position, enemy.transform.position) <= smalestDistance)
            {
                closetEnemy = enemy;
                isCollided = true;
                smalestDistance = math.distance(transform.position, enemy.transform.position);
            }
        }

        if (isCollided && closetEnemy != null)
        {
            Destroy(this.gameObject);
            closetEnemy.GetComponent<EnemyControl>().currentHitPoint -= BulletDamage;
        }
    }
    public void SetMoveDirection(Vector3 moveDirection)
    {
        this.moveDirection = moveDirection;
    }

}
