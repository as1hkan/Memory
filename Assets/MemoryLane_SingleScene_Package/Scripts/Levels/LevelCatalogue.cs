using UnityEngine;

[CreateAssetMenu(fileName = "LevelCatalogue", menuName = "MemoryLane/Level Catalogue")]
public class LevelCatalogue : ScriptableObject
{
    public LevelData[] levels;
}
