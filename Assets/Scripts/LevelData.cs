using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "MemoryLane/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Identity")]
    public int levelNumber = 1;

    [Header("Layout")]
    public Vector3[] cubePositions;

    public int endIndex = 0;

    [Header("Prefabs (per cube)")]
    public GameObject[] cubePrefabs;

    [Header("Visuals")]
    public Color[] cubeColors;
    public Color singleColor = Color.white;

    [Header("Win Animation")]
    public int[] fallingCubeIndices;
}
