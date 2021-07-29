using UnityEngine;
public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    EnemyManager Instance;
    [SerializeField] float offset =10;
    [SerializeField] float enemySpeed;
    private void Awake()
    {
        Instance = this;
        SpawnWaveEvent.Event += SpawnWave;
    }
    private void OnDisable()
    {
        SpawnWaveEvent.Event -= SpawnWave;
    }
    public void SpawnWave (SpawnWave spawnWave)
    {
        if (spawnWave == null)
            return;
        for (int i = 0; i < spawnWave.Amount; i++)
        {

        var enemy = Instantiate(enemyPrefab).GetComponent<Enemy>();

             enemy.IsAtFinalPosition = false;
             enemy.Speed = enemySpeed;
             enemy.SpawnWave = spawnWave;

             var direction = spawnWave.Road.Points[1] - spawnWave.Road.Points[0];
             Unity.Mathematics.math.normalize(direction);
             direction.y = 0;
             enemy.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

             enemy.transform.position = spawnWave.Road.Points[0] -direction*i * offset;
             enemy.ID = i;
        }

    }
}
