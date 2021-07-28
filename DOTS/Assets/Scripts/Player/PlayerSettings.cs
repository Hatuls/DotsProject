using UnityEngine;

[CreateAssetMenu (fileName = "Player Settings", menuName = "ScriptableObjects/Player/Settings")]
public class PlayerSettings : ScriptableObject
{
    public float VerticalSpeed;
    public float HorizontalSpeed;
    public Weapon StartingWeapon;
}
