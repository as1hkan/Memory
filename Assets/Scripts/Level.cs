using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newLevel", menuName = "MemoryLane/Level")]
public class Level : ScriptableObject
{
    public Vector3 CameraPosition;
    public List<Vector2> Cubes;
    public Vector2 StartPos;
    public Vector2 EndPos;
    public Color StartColor;
    public Color EndColor;
}
