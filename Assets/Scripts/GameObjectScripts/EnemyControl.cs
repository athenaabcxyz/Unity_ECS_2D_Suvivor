using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;
public class EnemyControl : MonoBehaviour
{

    public int enemiesType;
    public float moveSpeed;
    public int currentHitPoint;
    public int maxHP;
    public int damage;
    public Random random;
    public GameObject player;
    Animator animator;

    public GameObject EnemySpawner;
    // Start is called before the first frame update
    void Awake()
    {
        random = Random.CreateFromIndex((uint)this.GetInstanceID());
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        ChasePlayer(player.transform.position);
        SeftDamage();
    }

    public void ChasePlayer(Vector3 playerPosition)
    {
        Vector3 direction = playerPosition - transform.position;
        Vector3 move = Vector3.Normalize(direction) * moveSpeed * Time.deltaTime;
        Vector3 avoidForce = Vector3.zero;
        foreach (GameObject otherEnemy in EnemySpawner.GetComponent<EnemiesSpawner>().EnemyList)
        {
            if(this.gameObject.GetInstanceID()!=otherEnemy.GetInstanceID())
            {
                Vector3 dir = transform.position + move - otherEnemy.transform.position;
                var dist = math.distance(transform.position + move, otherEnemy.transform.position);
                if (dist <= 1.5f)
                {
                    avoidForce += dir / dist;
                }
            }        
        }
        transform.position += move + Vector3.Normalize(avoidForce) * Time.deltaTime * moveSpeed;

        EnemyAnimation(GetAimDirection(GetAngleFromVector(move)), move);
    }

    public void SeftDamage()
    {
        if (currentHitPoint <= 0)
        {
            EnemySpawner.GetComponent<EnemiesSpawner>().EnemyList.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
    }
    public void EnemyAnimation(RotationEnum rotation, Vector3 moveDirection)
    {
        if (moveDirection.x != 0 || moveDirection.y != 0)
        {
            animator.SetBool("isMoving", true);
            animator.SetBool("isIdle", false);

        }
        else
        {
            animator.SetBool("isMoving", false);
            animator.SetBool("isIdle", true);
        }
        switch (rotation)
        {
            case RotationEnum.aimUp:
                animator.SetBool("aimUp", true);
                animator.SetBool("aimDown", false);
                animator.SetBool("aimRight", false);
                animator.SetBool("aimLeft", false);
                animator.SetBool("aimUpLeft", false);
                animator.SetBool("aimUpRight", false);

                break;
            case RotationEnum.aimDown:
                animator.SetBool("aimUp", false);
                animator.SetBool("aimDown", true);
                animator.SetBool("aimRight", false);
                animator.SetBool("aimLeft", false);
                animator.SetBool("aimUpLeft", false);
                animator.SetBool("aimUpRight", false);
                break;
            case RotationEnum.aimRight:
                animator.SetBool("aimUp", false);
                animator.SetBool("aimDown", false);
                animator.SetBool("aimRight", true);
                animator.SetBool("aimLeft", false);
                animator.SetBool("aimUpLeft", false);
                animator.SetBool("aimUpRight", false);
                break;
            case RotationEnum.aimLeft:
                animator.SetBool("aimUp", false);
                animator.SetBool("aimDown", false);
                animator.SetBool("aimRight", false);
                animator.SetBool("aimLeft", true);
                animator.SetBool("aimUpLeft", false);
                animator.SetBool("aimUpRight", false);
                break;
            case RotationEnum.aimUpLeft:
                animator.SetBool("aimUp", false);
                animator.SetBool("aimDown", false);
                animator.SetBool("aimRight", false);
                animator.SetBool("aimLeft", false);
                animator.SetBool("aimUpLeft", true);
                animator.SetBool("aimUpRight", false);
                break;
            case RotationEnum.aimUpRight:
                animator.SetBool("aimUp", false);
                animator.SetBool("aimDown", false);
                animator.SetBool("aimRight", false);
                animator.SetBool("aimLeft", false);
                animator.SetBool("aimUpLeft", false);
                animator.SetBool("aimUpRight", true);
                break;
        }
    }
    RotationEnum GetAimDirection(float angleDegrees)
    {
        RotationEnum aimDirection;

        // Set player direction
        //Up Right
        if (angleDegrees >= 22f && angleDegrees <= 67f)
        {
            aimDirection = RotationEnum.aimUpRight;
        }
        // Up
        else if (angleDegrees > 67f && angleDegrees <= 112f)
        {
            aimDirection = RotationEnum.aimUp;
        }
        // Up Left
        else if (angleDegrees > 112f && angleDegrees <= 158f)
        {
            aimDirection = RotationEnum.aimUpLeft;
        }
        // Left
        else if ((angleDegrees <= 180f && angleDegrees > 158f) || (angleDegrees > -180 && angleDegrees <= -135f))
        {
            aimDirection = RotationEnum.aimLeft;
        }
        // Down
        else if ((angleDegrees > -135f && angleDegrees <= -45f))
        {
            aimDirection = RotationEnum.aimDown;
        }
        // Right
        else if ((angleDegrees > -45f && angleDegrees <= 0f) || (angleDegrees > 0 && angleDegrees < 22f))
        {
            aimDirection = RotationEnum.aimRight;
        }
        else
        {
            aimDirection = RotationEnum.aimRight;
        }

        return aimDirection;

    }

    float GetAngleFromVector(float3 vector)
    {

        float radians = Mathf.Atan2(vector.y, vector.x);

        float degrees = radians * Mathf.Rad2Deg;

        return degrees;

    }
}
