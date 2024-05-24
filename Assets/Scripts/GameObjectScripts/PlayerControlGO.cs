using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class PlayerControlGO : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float speed;
    public int maxHitPoint;
    public float hitCoolDown = 1f;
    public GameObject buttletPrefab;
    public float weaponShotICD = 0.5f;
    public float bulletSize;
    public float bulletSpeed;
    public float bulletSpread;
    public int deliveryDmg;
    public float range;
    public GameObject theScientist;
    public GameObject theThief;
    public GameObject rocketPrefab;
    public float shootICD;

    public GameObject spawner;


    //private value
    private PlayerWeaponBehavior weaponBehavior;
    private double nextShootICD;
    private double nextRocketICD;

    // Start is called before the first frame update
    void Start()
    {
        weaponBehavior = GetComponent<PlayerWeaponBehavior>();
        animator = GetComponent<Animator>();
        nextShootICD = Time.time;
        nextRocketICD = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }
    private void PlayerMovement()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");
        var mousePosition = Input.mousePosition;
        var input = math.normalizesafe(new Vector3(horizontalInput, verticalInput, 0)) * Time.deltaTime;
        Vector3 movement = input * speed;
        if (transform.position.x + movement.x <= -59 || transform.position.x + movement.x >= 59)
        {
            transform.position += new Vector3(0, movement.y, 0);
        }
        else

                if (transform.position.y + movement.y <= -32 || transform.position.y + movement.y >= 32)
        {
            transform.position += new Vector3(movement.x, 0, 0);
        }
        else
        {
            transform.position += movement;
        }
        Vector3 dir = mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var weaponDirection = mousePosition - Camera.main.WorldToScreenPoint(weaponBehavior.GetWeaponPosition());
        Debug.Log(GetAimDirection(GetAngleFromVector(dir)));
        ShootWeapon(weaponDirection, weaponBehavior.ShootPosition);
        ShootRocket(weaponDirection, weaponBehavior.ShootPosition);
        AnimatePlayer(GetAimDirection(GetAngleFromVector(dir)), movement);
        weaponBehavior.Aim(GetAimDirection(GetAngleFromVector(dir)), GetAngleFromVector(weaponDirection));
    }

    public RotationEnum GetAimDirection(float angleDegrees)
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

    private void ShootWeapon(Vector3 weaponDirection, Transform shootPosition)
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Time.time > nextShootICD)
            {
                var bullet = Instantiate(buttletPrefab);
                bullet.transform.position = shootPosition.position;
                bullet.GetComponent<BulletControl>().spawner = this.spawner;
                bullet.GetComponent<BulletControl>().SetMoveDirection(weaponDirection);
                nextShootICD = (float)Time.time + weaponShotICD;
            }
        }
    }
    private void ShootRocket(Vector3 weaponDirection, Transform shootPosition)
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (Time.time > nextRocketICD)
            {
                var bullet = Instantiate(rocketPrefab);
                bullet.transform.position = shootPosition.position;
                bullet.GetComponent<RocketControl>().spawner = this.spawner;
                bullet.GetComponent<RocketControl>().SetMoveDirection(weaponDirection);
                nextRocketICD = (float)Time.time + shootICD;
            }
        }
    }
    public float GetAngleFromVector(float3 vector)
    {

        float radians = Mathf.Atan2(vector.y, vector.x);

        float degrees = radians * Mathf.Rad2Deg;

        return degrees;

    }
    public void AnimatePlayer(RotationEnum rotation, Vector3 moveDirection)
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
}
