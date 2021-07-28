using Unity.Mathematics;
using Unity.Transforms;
using Unity.Entities;

public class DestructionSystem : ComponentSystem
{
  public const float thresholdDistance = 6f;
    const float offset = 5f;
    public  float3 playerPos ;
    protected override void OnUpdate()
    {
            if (GameManager.IsGameOver())
            return;

        if ((UnityEngine.Vector3)playerPos != PlayerManager.Body.position)
         playerPos = PlayerManager.Body.position;

        EnemyAndBulletSystem(playerPos);
    
    }
  
    private void EnemyAndBulletSystem(float3 playerPos)
    {


        Entities.WithAll<EnemyTag>().ForEach((Entity Enemy, ref Translation enemyPos,ref EnemyPowerUpHolder powerupData)
            =>
        {

            playerPos.y = enemyPos.Value.y;


            if (math.distance(playerPos, enemyPos.Value) <= thresholdDistance)
            {
                //player check if he touched the enemy ship

                PostUpdateCommands.DestroyEntity(Enemy);

                GameManager.EndGame();
            }

            float3 enemyPosition = enemyPos.Value;
            var data = powerupData.types;
            // bullet touched enemy
            Entities.WithAll<PlayerBulletTag>().ForEach((Entity bullet, ref Translation bulletPos)
                =>
            {

                if (math.distance(enemyPosition, bulletPos.Value) <= thresholdDistance) // bullets and the enemy are in colliding distance
                {
                    PlayerManager.AddScore();

                    if (data!= PowerUpManager.PowerUpType.None)
                    EntityHandler.CreatePowerUpEntity(data, enemyPosition);

                    PostUpdateCommands.DestroyEntity(Enemy);
                    PostUpdateCommands.DestroyEntity(bullet);


                }

                 if ( bulletPos.Value.z >= PlayerManager.TopScreenPoint + offset ) // bullet are to far away
                {
                    PostUpdateCommands.DestroyEntity(bullet);
                }




            });
        });
    }
}