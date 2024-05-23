using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyGunControl : MonoBehaviour
{
    public float ICD;
    public float bulletSpeed;
    public GameObject bulletPrefab;
    public GameObject player;

    private double ICDCounter;
    // Start is called before the first frame update
    void Start()
    {
        ICDCounter = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        var dir = player.transform.position - transform.position;
        if (ICDCounter<Time.time)
        {
            var bullet = Instantiate(bulletPrefab);
            bullet.transform.position = GetComponent<PlayerWeaponBehavior>().GetShootPosition();
            bullet.GetComponent<EnemyBulletControl>().SetMoveDirection(dir);
            bullet.GetComponent<EnemyBulletControl>().player  = player;          
            ICDCounter = Time.time + ICD;
        }
        GetComponent<PlayerWeaponBehavior>().Aim(GetAimDirection(GetAngleFromVector(dir)), GetAngleFromVector(dir));
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
    public float GetAngleFromVector(float3 vector)
    {

        float radians = Mathf.Atan2(vector.y, vector.x);

        float degrees = radians * Mathf.Rad2Deg;

        return degrees;

    }
}
