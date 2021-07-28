using Unity.Mathematics;
using Unity.Transforms;
using Unity.Entities;

public class ShipMovementSystem : ComponentSystem
{

    public const float distanceThreshHold = 4f;
    protected override void OnUpdate()
    {
        if (GameManager.IsGameOver())
            return;


        float3 direction = float3.zero;

        Road road = null;
        SpawnWave spawnWave = null;
        Entities.WithAll<EnemyTag>().ForEach((ref EnemyData enemyData, ref Translation trans, ref Rotation rot, ref FaceToward facing, ref MoveForward moveForward)
            =>
            {
                if (spawnWave == null || spawnWave.SpawnID != enemyData.spawnWaveID)
                {
                    spawnWave = WaveManager.Instance.GetSpawnWave(enemyData.spawnWaveID);
                    road = WaveManager.Instance.GetRoadPoint(enemyData.spawnWaveID);
                }


           
                if (facing.index > road.Points.Length) // we are at our final pos
                    return;
           

                if (facing.index == road.Points.Length) // if we at the last index then look down
                {
                    if (moveForward.toMove && math.distance(trans.Value, spawnWave.GetLastPosition(ref enemyData.enemyID)) < distanceThreshHold)
                    {
                      moveForward.toMove = false;                        
                        direction = new float3(trans.Value.x, trans.Value.y, trans.Value.z - 1f) - trans.Value;
                    direction.y = 0;
                    rot.Value = quaternion.LookRotation(direction, math.up());
                    facing.index++;
                    }
                    return;

                }
                
              if (facing.index  != road.Points.Length && math.distance(trans.Value, road.Points[facing.index]) < distanceThreshHold) // when we get close to the upcoming Point we move toward the next position
                {

                    float3 direction;


                    facing.index++;

                    if (facing.index == road.Points.Length)
                        direction = spawnWave.GetLastPosition(ref enemyData.enemyID) - trans.Value;
                    else
                    direction = road.Points[facing.index] - trans.Value;


                    direction.y = 0;
                    rot.Value = quaternion.LookRotation(direction, math.up());

              }


            }

        );
    }


}