using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu (fileName = "Wave", menuName = "ScriptableObjects/EnemyWave")]
public class SpawnWave : ScriptableObject
{
    [SerializeField] Road road;
    public Road Road => road;
    //[SerializeField] Enemy _enemy;
  [SerializeField]  int amount;
    public int Amount =>amount;
  public byte SpawnID;
    [SerializeField] float3 _startPos;
    float3[,] grid;
    [SerializeField] float widthGap;
    [SerializeField] float heghitGap;
    [SerializeField] int widthSlots;
    [SerializeField] int heightSlots;
    [SerializeField] bool drawGrid;





    public float3 GetLastPosition(ref int id)
    {
        if (grid == null)
        {
            grid = new float3[widthSlots, heightSlots];
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j] = new float3(_startPos.x + i * widthGap, 0f, _startPos.z + j * heghitGap);
                }
            }
        }


        return grid[id % (widthSlots), id % (heightSlots)];
    }

    public void DrawGrid()
    {
        if (drawGrid == false)
            return;


        Gizmos.color = Color.white;
        grid = new float3[widthSlots, heightSlots];
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                grid[i, j] = new float3(_startPos.x + i * widthGap, 0f, _startPos.z + j * heghitGap);
                Gizmos.DrawCube(grid[i, j], Vector3.one * 5f);
            }
        }
    }
}