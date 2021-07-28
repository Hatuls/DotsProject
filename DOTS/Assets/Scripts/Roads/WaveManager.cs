using UnityEngine;
using Unity.Mathematics;
public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    [SerializeField] SpawnWave[] spawnWaves;

    [SerializeField] int currentWave;
    private void Awake()
    {
        Instance = this;
        currentWave = 0;

    }
    private void Start()
    {
        EntityHandler.Instance.SpawnWave(spawnWaves[currentWave]);
        timer = 0;

    }
    [SerializeField] float timeBetweenWaves;
    [SerializeField] float reduceTimer;
    float timer;
    private void Update()
    {
        if (GameManager.IsGameOver())
            return;

        if (timer >= timeBetweenWaves)
        {
            currentWave++;
            if (currentWave >= spawnWaves.Length)
                currentWave = 0;
            EntityHandler.Instance.SpawnWave(spawnWaves[currentWave]);
            timer = 0;
            timeBetweenWaves -= reduceTimer;
            if (timeBetweenWaves <1)
            {
                timeBetweenWaves = 1f;
            }
        }
        timer += Time.deltaTime;
    }


    public SpawnWave GetSpawnWave (int index)
    {
     
            if (index < spawnWaves.Length && index >= 0)
                return spawnWaves[index];

            return null;
        
    }



    public Road GetRoadPoint(int index)
    {
            if (index < spawnWaves.Length && index >= 0)
              return spawnWaves[index].Road;

                return null;
    }


    #region gizmos
    private void OnDrawGizmos()
    {
        if (spawnWaves!= null)
        {

        for (int i = 0; i < spawnWaves.Length; i++)
        {
            spawnWaves[i].DrawGrid();


            if (spawnWaves[i].Road.ToShow)
            {
                Gizmos.color = spawnWaves[i].Road.clr;
                for (int j = 0; j < spawnWaves[i].Road.Points.Length; j++)
                    Gizmos.DrawCube(spawnWaves[i].Road.Points[j], Vector3.one * 5f);
            }
        }
    }
        }
    #endregion
}
