using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "MemoryLane/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Identity")]
    public int levelNumber = 1;

    [Header("Layout")]
    public Vector3[] cubePositions;

    [Tooltip("کدام اندیس در cubePositions انتهای مسیر است؟")]
    public int endIndex = 0;

    [Header("Prefabs (per cube)")]
    [Tooltip("Prefab هر cube در این مرحله (در صورت خالی بودن از اولین Prefab کلی استفاده می‌شود)")]
    public GameObject[] cubePrefabs;

    [Header("Visuals")]
    public Color[] cubeColors;
    public Color singleColor = Color.white;

    [Header("Win Animation")]
    public int[] fallingCubeIndices;
}
