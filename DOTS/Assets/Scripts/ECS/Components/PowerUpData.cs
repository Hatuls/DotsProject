using Unity.Entities;

[GenerateAuthoringComponent]
public struct PowerUpData : IComponentData
{
    public PowerUpManager.PowerUpType Type;
}
