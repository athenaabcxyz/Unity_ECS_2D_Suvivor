using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerWeaponBehavior : MonoBehaviour
{
    public Transform weaponRotationPointTransform;
    public Transform ShootPosition;

    public void Aim(RotationEnum AimDirection, float aimAngle)
    {
        weaponRotationPointTransform.eulerAngles = new Vector3(0f, 0f, aimAngle);

        // Flip weapon transform based on player direction
        switch (AimDirection)
        {
            case RotationEnum.aimLeft:
            case RotationEnum.aimUpLeft:
                weaponRotationPointTransform.localScale = new Vector3(1f, -1f, 0f);
                break;

            case RotationEnum.aimUp:
            case RotationEnum.aimUpRight:
            case RotationEnum.aimRight:
            case RotationEnum.aimDown:
                weaponRotationPointTransform.localScale = new Vector3(1f, 1f, 0f);
                break;
        }
    }

    public Vector3 GetShootPosition()
    {
        return ShootPosition.position;
    }
    public Vector3 GetWeaponPosition()
    {
        return weaponRotationPointTransform.position;
    }

}
