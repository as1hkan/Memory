using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Data")]
    public LevelCatalogue catalogue;
    public int startLevelIndex = 0;

    [Header("Prefabs (global pool)")]
    [Tooltip("Prefabهایی که در همه‌ی مراحل ممکن است استفاده شوند")]
    public List<GameObject> cubePrefabs = new List<GameObject>();

    [Header("Parent & References")]
    public Transform levelRoot;
    public SimpleGridMovement player;
    public SingleSceneGameManager gameManager;

    [Header("Masks")]
    public LayerMask blockMask;
    public LayerMask endMask;

    private int currentLevelIndex = -1;
    private readonly List<GameObject> spawnedCubes = new List<GameObject>();
    private readonly List<GameObject> spawnedFalling = new List<GameObject>();

    private void Awake()
    {
        LoadLevel(startLevelIndex);
    }

    public void LoadLevel(int index)
    {
        if (catalogue == null || catalogue.levels == null || catalogue.levels.Length == 0)
        {
            Debug.LogError("❌ LevelManager: Catalogue is empty!");
            return;
        }

        if (index < 0 || index >= catalogue.levels.Length)
            index = 0;

        ClearCurrentLevel();
        currentLevelIndex = index;
        LevelData data = catalogue.levels[currentLevelIndex];

        // 🧱 ساخت بلاک‌ها بر اساس Prefabهای هر مرحله
        for (int i = 0; i < data.cubePositions.Length; i++)
        {
            GameObject prefabToUse = null;

            if (data.cubePrefabs != null && i < data.cubePrefabs.Length && data.cubePrefabs[i] != null)
            {
                // Prefab مخصوص این cube در LevelData مشخص شده
                prefabToUse = data.cubePrefabs[i];
            }
            else if (cubePrefabs.Count > 0)
            {
                // اگر Prefab خاصی برای این cube تنظیم نشده، اولین prefab در لیست کلی استفاده می‌شود
                prefabToUse = cubePrefabs[0];
            }

            if (prefabToUse == null)
            {
                Debug.LogError($"🚫 Prefab برای cube شماره {i} پیدا نشد!");
                continue;
            }

            GameObject cube = Instantiate(prefabToUse, data.cubePositions[i], Quaternion.identity, levelRoot);
            cube.name = $"Cube_{i + 1}";

            // 🎨 رنگ
            Renderer rend = cube.GetComponent<Renderer>();
            if (rend != null)
            {
                if (data.cubeColors != null && data.cubeColors.Length == data.cubePositions.Length)
                    rend.material.color = data.cubeColors[i];
                else
                    rend.material.color = data.singleColor;
            }

            // 🧩 لایه
            int layer = (i == data.endIndex) ? MaskToLayer(endMask) : MaskToLayer(blockMask);
            if (layer >= 0) cube.layer = layer;

            spawnedCubes.Add(cube);
        }

        // 🧱 تعیین بلاک‌هایی که باید بعد از بردن بیفتند
        if (data.fallingCubeIndices != null && data.fallingCubeIndices.Length > 0)
        {
            foreach (int idx in data.fallingCubeIndices)
            {
                if (idx >= 0 && idx < spawnedCubes.Count)
                    spawnedFalling.Add(spawnedCubes[idx]);
            }
        }

        // 🎮 بایند کردن به GameManager
        if (gameManager != null)
        {
            gameManager.BindRuntime(
                spawnedCubes.ToArray(),
                spawnedFalling.ToArray(),
                player,
                blockMask,
                endMask,
                currentLevelIndex
            );
        }
    }

    public void LoadNextLevel()
    {
        int next = currentLevelIndex + 1;
        if (catalogue != null && next >= catalogue.levels.Length)
            next = catalogue.levels.Length - 1;

        LoadLevel(next);
    }

    public void LoadPrevLevel()
    {
        int prev = Mathf.Max(0, currentLevelIndex - 1);
        LoadLevel(prev);
    }

    public void ClearCurrentLevel()
    {
        for (int i = levelRoot.childCount - 1; i >= 0; i--)
            Destroy(levelRoot.GetChild(i).gameObject);

        spawnedCubes.Clear();
        spawnedFalling.Clear();
    }

    private int MaskToLayer(LayerMask mask)
    {
        int m = mask.value;
        if (m == 0) return -1;
        int layer = 0;
        while ((m & 1) == 0)
        {
            m >>= 1;
            layer++;
        }
        return layer;
    }
    public int GetCurrentIndex()
    {
        return currentLevelIndex;
    }
}
