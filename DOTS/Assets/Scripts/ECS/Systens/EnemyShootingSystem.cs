using Unity.Mathematics;
using Unity.Transforms;
using Unity.Entities;

public class EnemyShootingSystem : ComponentSystem
{

    [UnityEngine.SerializeField]
    Entity EnemyBullet;

    protected override void OnStartRunning()
    {
        base.OnStartRunning();

    }

    protected override void OnUpdate()
    {
        float3 offset = new float3 ( 0, 0, -1f );
        float deltaTime = Time.DeltaTime;
        EnemyBullet = EntityHandler.EnemyBullet;
        Entities.WithAll<EnemyTag>().ForEach((ref Translation enemyPos,ref Rotation enemyRot,ref MoveForward isMoving, ref EnemyShootingSpeed shooting)
        =>
        {
            if (isMoving.toMove == false)
            {
                if (shooting.shootingTime == 0)
                {

                    shooting.shootingTime = UnityEngine.Random.Range(shooting.minShootingTime, shooting.maxShootingTime);

                }


                if (shooting.currentTime < shooting.shootingTime)
                {
                    shooting.currentTime += deltaTime;

                }
                else
                {
               var bullet =     EntityManager.Instantiate(EnemyBullet);

                    EntityManager.SetComponentData(bullet, new Rotation { Value = enemyRot.Value });
                    EntityManager.SetComponentData(bullet, new Translation { Value = enemyPos.Value + offset});
                    EntityManager.SetComponentData(bullet, new MoveForward { Speed = 40f, toMove =true});

                    shooting.currentTime = 0;
                }
            }
        }
        
        
        
        );



    }


}