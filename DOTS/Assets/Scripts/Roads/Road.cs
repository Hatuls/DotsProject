using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "Road", menuName = "ScriptableObjects/Pattern Road")]
[System.Serializable]
public class Road : ScriptableObject
{
    [SerializeField] float3[] points;
    public float3[] Points => points;

    public Color clr;

    public bool ToShow;
  
}
