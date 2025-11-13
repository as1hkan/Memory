using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private SimpleGridMovement player;
    [SerializeField] private List<Level> levelsData;
    [SerializeField] private Transform parent;

    [Header("Prefabs")]
    [SerializeField] private GameObject CubePrefab;
    [SerializeField] private GameObject CubeStart;
    [SerializeField] private GameObject CubeEnd;

    private List<GameObject> localPrivate = new List<GameObject>();
    private List<GameObject> BlockToFall = new List<GameObject>();

    private GameObject currentLevelGO;

    public void Load(int index)
    {
        if (currentLevelGO != null)
            Destroy(currentLevelGO);

        currentLevelGO = new GameObject("LEVEL_" + index);
        currentLevelGO.transform.SetParent(parent);

        localPrivate.Clear();
        BlockToFall.Clear();

        Level lvl = levelsData[index - 1];
        if (lvl == null) return;

        // Start block
        GameObject startObj = Instantiate(
            CubeStart,
            new Vector3(lvl.StartPos.x, 0, lvl.StartPos.y),
            Quaternion.identity,
            currentLevelGO.transform
        );

        BlockToFall.Add(startObj);

        // Path blocks
        foreach (var pos in lvl.Cubes)
        {
            GameObject cube = Instantiate(
                CubePrefab,
                new Vector3(pos.x, 0, pos.y),
                Quaternion.identity,
                currentLevelGO.transform
            );

            localPrivate.Add(cube);
        }

        // End block
        GameObject endObj = Instantiate(
            CubeEnd,
            new Vector3(lvl.EndPos.x, 0, lvl.EndPos.y),
            Quaternion.identity,
            currentLevelGO.transform
        );

        BlockToFall.Add(endObj);

        // Color gradient
        for (int i = 0; i < localPrivate.Count; i++)
        {
            float t = (float)i / Mathf.Max(1, localPrivate.Count - 1);
            Color c = Color.Lerp(lvl.StartColor, lvl.EndColor, t);

            MeshRenderer r = localPrivate[i].GetComponent<MeshRenderer>();
            if (r != null) r.material.color = c;

            BlockToFall.Add(localPrivate[i]);
        }

        BlockToFall.Add(player.gameObject);

        // Export to GameManager
        GameManager.instance.blocks = localPrivate.ToArray();
        GameManager.instance.blocksToFall = BlockToFall.ToArray();
        GameManager.instance.StartPoint = startObj.transform;
        GameManager.instance.EndPoint = endObj.transform;
    }
}
