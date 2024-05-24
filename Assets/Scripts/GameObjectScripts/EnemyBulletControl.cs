using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletControl : MonoBehaviour
{
    public int BulletDamage;
    public float BulletSpeed;
    public GameObject healthPrefab;
    public GameObject spawner;
    private Vector3 moveDirection;
    public GameObject player;

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
        transform.position += Vector3.Normalize(moveDirection) * BulletSpeed * Time.deltaTime;
        if (transform.position.x < -60 || transform.position.x > 60 || transform.position.y > 35 || transform.position.y < -35)
        {
            Destroy(this.gameObject);
        }
    }

    public void BulletDamageCall()
    {
       
        if (Vector3.Distance(transform.position, player.transform.position)<=0.8f)
        {
            Destroy(this.gameObject);
            player.GetComponent<PlayerControlGO>().maxHitPoint -= BulletDamage;
        }
    }
    public void SetMoveDirection(Vector3 moveDirection)
    {
        this.moveDirection = moveDirection;
    }
}
