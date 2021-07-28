
using Unity.Entities;

[GenerateAuthoringComponent]
public struct EnemyPowerUpHolder : IComponentData
{
    public PowerUpManager.PowerUpType types;
}
