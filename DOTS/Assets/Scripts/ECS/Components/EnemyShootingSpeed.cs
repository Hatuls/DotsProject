using Unity.Entities;

[GenerateAuthoringComponent]
public struct EnemyShootingSpeed : IComponentData {
    public float minShootingTime; 
    public float  maxShootingTime;
    public float currentTime; 
    public float shootingTime;
}