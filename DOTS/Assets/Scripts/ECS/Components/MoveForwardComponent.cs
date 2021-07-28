using Unity.Entities;
[GenerateAuthoringComponent]
public struct MoveForward : IComponentData
{
    public bool toMove;
    public float Speed;
}
