using Unity.Entities;
using UnityEngine;
using Unity.Collections;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;
using System;

public class EntityHandler :MonoBehaviour
{


    public static EntityHandler Instance;
    
    private EntityManager _entityManager;
    [SerializeField] private GameObject _enemyPrefab;

    [SerializeField] float _speedShip;
    [SerializeField] float spawnOffsetGapBetweenSpawning;

    [SerializeField] Mesh _powerUpMesh;
    [SerializeField] Material powerUpSpeedMat;
    [SerializeField]Material powerUpAmmoMat;

    [SerializeField] Vector3 _startingPos;
    private Entity _enemyEntitiyPrefab;


    [SerializeField] GameObject _powerUpPrefab;
    private Entity _powerUpEntity;


    [SerializeField] GameObject _projectilePrefab;
    private Entity _projectileEntitiyPrefab; 
    
    [SerializeField] GameObject _enemyBullet;
    private Entity _enemyProjectileEntitiyPrefab;

    public static Entity EnemyBullet => Instance._enemyProjectileEntitiyPrefab;
    private void OnDestroy()
    {

        SpawnWaveEvent.Event -= SpawnWave;
        BulletInstantiateEvent.Event -= SpawnPlayerBullet;
    }
    private void Awake()
    {
        Init();
    }
    public void Init()
    {
     
        BulletInstantiateEvent.Event += SpawnPlayerBullet;
        SpawnWaveEvent.Event += SpawnWave;
        Instance = this;
        
            _entityManager =World.DefaultGameObjectInjectionWorld.EntityManager;
            var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);

            _enemyEntitiyPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(_enemyPrefab, settings);

            _projectileEntitiyPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(_projectilePrefab, settings);

            _powerUpEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(_powerUpPrefab, settings);


            _enemyProjectileEntitiyPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(_enemyBullet, settings);
      
    }



    public void SpawnWave(SpawnWave spawnWave)
    {
        if (GameManager.IsDots == false)
            return;

        NativeArray<Entity> enemyArray = new NativeArray<Entity>(spawnWave.Amount, Allocator.Temp);

        float3 direction = math.normalize(spawnWave.Road.Points[1] - spawnWave.Road.Points[0]);
        direction.y = 0;

        for (int i = 0; i < enemyArray.Length; i++)
        {

            UIManager.RegisterEnemy();
            enemyArray[i] = _entityManager.Instantiate(_enemyEntitiyPrefab);

            _entityManager.SetComponentData(
             enemyArray[i],
             new FaceToward
             {
                 index = 1
             }
             );

            _entityManager.SetComponentData(
       enemyArray[i],
       new Rotation
       {
           Value = quaternion.LookRotation(direction, math.up())
       }
       );

            _entityManager.SetComponentData(
                enemyArray[i],
                new Translation
                {
                    Value = (spawnWave.Road.Points[0] + -(direction) * i * spawnOffsetGapBetweenSpawning)
                }
                );

            _entityManager.SetComponentData(
                enemyArray[i],
                new MoveForward
                {
                    Speed = _speedShip,
                    toMove = true
                }
                );


            _entityManager.SetComponentData(
                enemyArray[i],
                new EnemyData
                {
                    enemyID = i,
                    spawnWaveID = spawnWave.SpawnID
                }
                );

            _entityManager.SetComponentData(
            enemyArray[i],
            new EnemyPowerUpHolder
            {
                types = (PowerUpManager.PowerUpType)(UnityEngine.Random.Range(0, Enum.GetNames(typeof(PowerUpManager.PowerUpType)).Length))
            }
            ) ;

            _entityManager.SetComponentData(
enemyArray[i],
new EnemyShootingSpeed
{
    minShootingTime = 2f,
    maxShootingTime = 5f
}
) ;
        }



        enemyArray.Dispose();
    }

    public void SpawnPlayerBullet(Weapon weapon,quaternion quaternion, int currentLevel)
    {
        if (GameManager.IsDots == false)
            return;
        switch (weapon.WeaponType)
        {
            case Weapon.WeaponTypeEnum.Projectile:

                SpawnProjectile( (ProjectileWeapon)weapon,  quaternion, currentLevel);
                break;
            case Weapon.WeaponTypeEnum.Laser:
                break;
            case Weapon.WeaponTypeEnum.Rockets:
                break;
            default:
                break;
        }
    }

    private void SpawnProjectile(ProjectileWeapon weapon, quaternion quaternion, int currentLevel)
    {
        if (GameManager.IsDots == false)
            return;
        NativeArray<Entity> bullets = new NativeArray<Entity>(currentLevel, Allocator.Temp);
        for (int i = 0; i < bullets.Length; i++)
        {
        UIManager.RegisterBullt();

            bullets[i] = _entityManager.Instantiate(_projectileEntitiyPrefab);

            float3 pos = PlayerManager.Body.position + PlayerManager.Body.localPosition.y * Vector3.forward;

            if (currentLevel > 1)
            {
                pos= new float3
                {
                    x= (pos.x +i*weapon.GapBetween) - (weapon.GapBetween * (currentLevel-1))/2
                    ,y =pos.y,
                    z=pos.z
                };
            }

            _entityManager.SetComponentData(
                bullets[i],
                new Translation
                {
                    Value = pos
                }) ;


            _entityManager.SetComponentData(
                bullets[i],
                new MoveForward { Speed = weapon.ProjectileSpeed, toMove = true });
        }

        bullets.Dispose();
    }


    public static void CreatePowerUpEntity(PowerUpManager.PowerUpType type, float3 Position)
    {
        if (GameManager.IsDots == false)
            return;
        var powerUp = Instance._entityManager.Instantiate(Instance._powerUpEntity);


        Instance._entityManager.SetComponentData(powerUp, new Translation { Value =Position });

        Instance._entityManager.SetComponentData(powerUp, new Rotation { Value = quaternion.LookRotation((-Vector3.forward), math.up()) });

        Instance._entityManager.SetComponentData(powerUp, new PowerUpData { Type = type });

        Instance._entityManager.AddSharedComponentData(powerUp, new RenderMesh
        {
            receiveShadows = false,
            castShadows = UnityEngine.Rendering.ShadowCastingMode.Off,
            material = Instance.GetPowerUpMaterial(type),
            mesh = Instance._powerUpMesh
        });

        Instance._entityManager.SetComponentData(
     powerUp,
      new MoveForward { Speed = 30f, toMove = true });
    }


    public Material GetPowerUpMaterial(PowerUpManager.PowerUpType type)
    {
        if (GameManager.IsDots == false)
            return null;
        switch (type)
        {

            case PowerUpManager.PowerUpType.Projectile:
                return powerUpAmmoMat;
            case PowerUpManager.PowerUpType.Speed:

                return powerUpSpeedMat;
            case PowerUpManager.PowerUpType.None:
            default:
                return null;
        }
    }



    public static void ResetEntities()
    {

//    Instance._entityManager.DestroyAndResetAllEntities();
    }
    #region Normal Entity Instantiantion

    // {
    //[SerializeField] Mesh _mesh;
    //  [SerializeField] Material _material;
    /// Summarty of how to do Pure ECS
    //private void Start()
    //{


    //    // all entites, components, and systems exists in a World. each world has one EntityManager. Though very verbose' this line simly grabs a refernce to it.
    //    _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

    //    // this associates certain data types together.
    //    // in this case Translation(movement), Rotation, RenderMesh,RenderBounds amd localToWorld, form the Archetype.
    //    // those are the components we use for the entity
    //    EntityArchetype _archeType = _entityManager.CreateArchetype(
    //        typeof(Translation),
    //        typeof(Rotation),
    //        typeof(RenderMesh),
    //        typeof(RenderBounds),
    //        typeof(LocalToWorld)
    //        );

    //    // Generate Entity. and initilaize it with the Archetype (basically inserting the components into the entity)
    //    Entity entity = _entityManager.CreateEntity(_archeType);


    //    // addcomponentdata to add specific values
    //    _entityManager.AddComponentData(
    //        entity,
    //        new Translation { Value = new float3(0f, 0f, 0f) }
    //        );


    //    // addcomponentdata to add specific values
    //    _entityManager.AddComponentData(
    //        entity,
    //        new Rotation { Value = quaternion.EulerXYZ(new float3(0, 45f, 0f)) }
    //        );


    //    // addsharedComponent to share same values across entites
    //    _entityManager.AddSharedComponentData(

    //        entity,
    //        new RenderMesh
    //        {
    //            mesh = _mesh,
    //            material = _material

    //        }
    //    );

    //}

    //   }
    #endregion
}
