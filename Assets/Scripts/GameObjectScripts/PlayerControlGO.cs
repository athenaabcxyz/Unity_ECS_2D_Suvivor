using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class PlayerControlGO : MonoBehaviour
{
    public Animator animator;
    public float speed;
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

    private IAnimationState currentAimState;
    private IAnimationState currentIdleState;


    //private value
    private PlayerWeaponBehavior weaponBehavior;
    private double nextShootICD;
    private double nextRocketICD;
    private IShootStrategy shootStrategy;

    // Start is called before the first frame update
    void Start()
    {
        weaponBehavior = GetComponent<PlayerWeaponBehavior>();
        animator = GetComponent<Animator>();
        nextShootICD = Time.time;
        nextRocketICD = Time.time;
        shootStrategy = new ShootBulletStrategy();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }

    public void SwitchToBulletStrategy()
    {
        shootStrategy = new ShootBulletStrategy();
    }

    public void SwitchToRocketStrategy()
    {
        shootStrategy = new ShootRocketStrategy();
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
            ChangeIdleState(new MovingAnimationState(this));

        }
        else
        {
            ChangeIdleState(new IdleAnimationState(this));
        }
        switch (rotation)
        {
            case RotationEnum.aimUp:
               ChangeAimState(new AimUpAnimationState(this));

                break;
            case RotationEnum.aimDown:
                ChangeAimState(new AimDownAnimationState(this));
                break;
            case RotationEnum.aimRight:
                ChangeAimState(new AimRightAnimationState(this));
                break;
            case RotationEnum.aimLeft:
                ChangeAimState(new AimLeftAnimationState(this));
                break;
            case RotationEnum.aimUpLeft:
                ChangeAimState(new AimUpLeftAnimationState(this));
                break;
            case RotationEnum.aimUpRight:
                ChangeAimState(new AimUpRightAnimationState(this));
                break;
        }
    }
    public void ChangeAimState(IAnimationState newAnimationState)
    {
        currentAimState = newAnimationState;
        currentAimState.UpdateState();
    }

     public void ChangeIdleState(IAnimationState newAnimationState)
    {
        currentIdleState = newAnimationState;
        currentIdleState.UpdateState();
    }

   
}

public interface IAnimationState
{
    void UpdateState();
}

public class IdleAnimationState : IAnimationState
{
    private PlayerControlGO playerControl;

    public IdleAnimationState(PlayerControlGO playerControl)
    {
        this.playerControl = playerControl;
    }

    public void UpdateState()
    {
        playerControl.animator.SetBool("isMoving", false);
        playerControl.animator.SetBool("isIdle", true);

        // Additional animation state-specific logic for idle state
    }
}

public class MovingAnimationState : IAnimationState
{
    private PlayerControlGO playerControl;

    public MovingAnimationState(PlayerControlGO playerControl)
    {
        this.playerControl = playerControl;
    }

    public void UpdateState()
    {
        playerControl.animator.SetBool("isMoving", true);
        playerControl.animator.SetBool("isIdle", false);

        // Additional animation state-specific logic for moving state
    }
}

public class AimDownAnimationState : IAnimationState
{
    private PlayerControlGO playerControl;

    public AimDownAnimationState(PlayerControlGO playerControl)
    {
        this.playerControl = playerControl;
    }

    public void UpdateState()
    {
        playerControl.animator.SetBool("aimUp", false);
        playerControl.animator.SetBool("aimDown", true);
        playerControl.animator.SetBool("aimRight", false);
        playerControl.animator.SetBool("aimLeft", false);
        playerControl.animator.SetBool("aimUpLeft", false);
        playerControl.animator.SetBool("aimUpRight", false);
    }
}

public class AimRightAnimationState : IAnimationState
{
    private PlayerControlGO playerControl;

    public AimRightAnimationState(PlayerControlGO playerControl)
    {
        this.playerControl = playerControl;
    }

    public void UpdateState()
    {
        playerControl.animator.SetBool("aimUp", false);
        playerControl.animator.SetBool("aimDown", false);
        playerControl.animator.SetBool("aimRight", true);
        playerControl.animator.SetBool("aimLeft", false);
        playerControl.animator.SetBool("aimUpLeft", false);
        playerControl.animator.SetBool("aimUpRight", false);
    }
}

public class AimLeftAnimationState : IAnimationState
{
    private PlayerControlGO playerControl;

    public AimLeftAnimationState(PlayerControlGO playerControl)
    {
        this.playerControl = playerControl;
    }

    public void UpdateState()
    {
        playerControl.animator.SetBool("aimUp", false);
        playerControl.animator.SetBool("aimDown", false);
        playerControl.animator.SetBool("aimRight", false);
        playerControl.animator.SetBool("aimLeft", true);
        playerControl.animator.SetBool("aimUpLeft", false);
        playerControl.animator.SetBool("aimUpRight", false);
    }
}

public class AimUpLeftAnimationState : IAnimationState
{
    private PlayerControlGO playerControl;

    public AimUpLeftAnimationState(PlayerControlGO playerControl)
    {
        this.playerControl = playerControl;
    }

    public void UpdateState()
    {
        playerControl.animator.SetBool("aimUp", false);
        playerControl.animator.SetBool("aimDown", false);
        playerControl.animator.SetBool("aimRight", false);
        playerControl.animator.SetBool("aimLeft", false);
        playerControl.animator.SetBool("aimUpLeft", true);
        playerControl.animator.SetBool("aimUpRight", false);
    }
}

public class AimUpRightAnimationState : IAnimationState
{
    private PlayerControlGO playerControl;

    public AimUpRightAnimationState(PlayerControlGO playerControl)
    {
        this.playerControl = playerControl;
    }

    public void UpdateState()
    {
        playerControl.animator.SetBool("aimUp", false);
        playerControl.animator.SetBool("aimDown", false);
        playerControl.animator.SetBool("aimRight", false);
        playerControl.animator.SetBool("aimLeft", false);
        playerControl.animator.SetBool("aimUpLeft", false);
        playerControl.animator.SetBool("aimUpRight", true);
    }
}

public class AimUpAnimationState : IAnimationState
{
    private PlayerControlGO playerControl;

    public AimUpAnimationState(PlayerControlGO playerControl)
    {
        this.playerControl = playerControl;
    }

    public void UpdateState()
    {
        playerControl.animator.SetBool("aimUp", true);
        playerControl.animator.SetBool("aimDown", false);
        playerControl.animator.SetBool("aimRight", false);
        playerControl.animator.SetBool("aimLeft", false);
        playerControl.animator.SetBool("aimUpLeft", false);
        playerControl.animator.SetBool("aimUpRight", false);
    }
}


public interface IShootStrategy
{
    void Shoot(Vector3 weaponDirection, Transform shootPosition, GameObject bulletPrefab, float weaponShotICD, GameObject spawner, float nextShootICD);
}

public class ShootBulletStrategy : MonoBehaviour, IShootStrategy
{
    public void Shoot(Vector3 weaponDirection, Transform shootPosition, GameObject bulletPrefab, float weaponShotICD, GameObject spawner, float nextShootICD)
    {
        var bullet = Instantiate(bulletPrefab);
        bullet.transform.position = shootPosition.position;
        bullet.GetComponent<BulletControl>().spawner = spawner;
        bullet.GetComponent<BulletControl>().SetMoveDirection(weaponDirection);
        nextShootICD = (float)Time.time + weaponShotICD;
    }
}

public class ShootRocketStrategy : MonoBehaviour, IShootStrategy
{
    public void Shoot(Vector3 weaponDirection, Transform shootPosition, GameObject bulletPrefab, float weaponShotICD, GameObject spawner, float nextShootICD)
    {
        var rocket = Instantiate(bulletPrefab);
        rocket.transform.position = shootPosition.position;
        rocket.GetComponent<RocketControl>().spawner = spawner;
        rocket.GetComponent<RocketControl>().SetMoveDirection(weaponDirection);
        nextShootICD = (float)Time.time + weaponShotICD;
    }
}