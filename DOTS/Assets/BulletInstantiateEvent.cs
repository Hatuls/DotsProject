using Unity.Mathematics;

public static class BulletInstantiateEvent
{
    public static UnityEngine.Events.UnityAction<Weapon,quaternion,int> Event;
    public static void RaiseEvent(Weapon weapon, quaternion rotation, int level)
=> Event?.Invoke(weapon, rotation, level);
}