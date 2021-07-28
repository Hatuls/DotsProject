using UnityEngine;

[CreateAssetMenu(fileName = "Projectile Weapon", menuName = "ScriptableObjects/Weapons/Projectile Weapon")]
public class ProjectileWeapon : Weapon
{
    public override WeaponTypeEnum WeaponType => WeaponTypeEnum.Projectile;



    [SerializeField] private float _gapBetween;
    public float GapBetween => _gapBetween;





    [SerializeField] private float _projectileSpeed;
    public float ProjectileSpeed { get => _projectileSpeed;  }
}