using UnityEngine;

public abstract class Weapon : ScriptableObject
{
    public float CoolDownSpeed;
    public enum WeaponTypeEnum { Projectile, Laser, Rockets };
    public abstract WeaponTypeEnum WeaponType { get; }
    public int Damage;

   
}
