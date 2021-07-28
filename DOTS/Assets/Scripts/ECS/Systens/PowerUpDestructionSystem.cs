using Unity.Mathematics;
using Unity.Transforms;
using Unity.Entities;

public class PowerUpDestructionSystem : ComponentSystem
{
    const float offset = 5f;
    protected override void OnUpdate()
    {
        PowerUpSystem();
    }

    private void PowerUpSystem()
    {
        float3 playerPos = PlayerManager.Body.position;

        Entities.WithAll<PowerUpTag>().ForEach((Entity powerUp, ref Translation powerUpPos, ref PowerUpData type) => {

            if (math.distance(powerUpPos.Value, playerPos) <= DestructionSystem.thresholdDistance) // player collider with the powerup
            {
                PowerUpManager.GainedPowerUp(type.Type);
                PostUpdateCommands.DestroyEntity(powerUp);
                return;
            }


            if (powerUpPos.Value.z < PlayerManager.BottomScreenPoint - offset)
            {

                PostUpdateCommands.DestroyEntity(powerUp);
            }


        });
    }
}
