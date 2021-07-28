using Unity.Mathematics;
using Unity.Transforms;
using Unity.Entities;

public class EnemyBulletDestructionSystem : ComponentSystem
{
    float offset = 4f;
    protected override void OnUpdate()
    {
        float3 playerPos = PlayerManager.Body.position;

        Entities.WithAll<EnemyBulletTag>().ForEach((Entity bullet, ref Translation bulletPos)
            =>
        {
            if (math.distance(bulletPos.Value, playerPos) <= DestructionSystem.thresholdDistance) // bullet hit the enemy
            {
                GameManager.EndGame();
                PostUpdateCommands.DestroyEntity(bullet);
            }

            if (bulletPos.Value.z <= PlayerManager.BottomScreenPoint - offset) // bullet Suprassed the lowest Point
            {
                PostUpdateCommands.DestroyEntity(bullet);
            }
        }
            
            
            );
    }
}
