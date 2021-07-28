using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
  public enum PowerUpType { None=0, Projectile, Speed };

    public static PowerUpManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    internal static void GainedPowerUp(PowerUpType type)
    {
        Instance.AssignPowerUp(type);
    }

    private void AssignPowerUp(PowerUpType type)
    {
        switch (type)
        {
              
            case PowerUpType.Projectile:
                PlayerShootings.WeaponLevel++;
                break;
            case PowerUpType.Speed:
                PlayerMovement.SpeedBuffer += 2f;
                break;
            case PowerUpType.None:
            default:
                return;
        }
    }
}
