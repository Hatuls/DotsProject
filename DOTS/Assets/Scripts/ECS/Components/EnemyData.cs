
using Unity.Entities;

[GenerateAuthoringComponent]
public struct EnemyData : IComponentData
{
    public int enemyID;
    public byte spawnWaveID;

}
