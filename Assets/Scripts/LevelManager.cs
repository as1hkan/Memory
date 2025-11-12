using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private SimpleGridMovement player;
    [SerializeField] private List<Level> levelsData;
    [SerializeField] private Transform parrent;
    //[SerializeField] private Transform camera;
    [Header("Prefabs")]
    [SerializeField] private GameObject CubePrefab;
    [SerializeField] private GameObject CubeStart;
    [SerializeField] private GameObject CubeEnd;

    private List<GameObject> localPrivate = new List<GameObject>();
    private List<GameObject> BlockToFall = new List<GameObject>();


    public void Load(int index)
    {
        // Level
        Level currentLevel = levelsData[index - 1];

        if (currentLevel == null) return;

        // Camera
        //camera.position = currentLevel.CameraPosition;

        // Instantiate
        BlockToFall.Add(Instantiate(CubeStart, new Vector3(currentLevel.StartPos.x, 0, currentLevel.StartPos.y), Quaternion.identity, parrent));

        for (int i = 0; i < currentLevel.Cubes.Count; i++)
        {
            localPrivate.Add(Instantiate(CubePrefab, new Vector3(currentLevel.Cubes[i].x, 0, currentLevel.Cubes[i].y), Quaternion.identity, parrent));
        }

        BlockToFall.Add(Instantiate(CubeEnd, new Vector3(currentLevel.EndPos.x, 0, currentLevel.EndPos.y), Quaternion.identity, parrent));

        // Color
        for (int i = 0; i < localPrivate.Count; i++)
        {
            float t = (localPrivate.Count == 1) ? 0f : (float)i / (localPrivate.Count - 1);
            Color currentColor = Color.Lerp(currentLevel.StartColor, currentLevel.EndColor, t);

            Renderer rend = localPrivate[i].GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material.color = currentColor;
            }
        }

        foreach (var block in localPrivate)
        {
            BlockToFall.Add(block);
        }
        BlockToFall.Add(player.transform.gameObject);

        GameManager.instance.blocksToFall = BlockToFall.ToArray();
        GameManager.instance.blocks = localPrivate.ToArray();
    }

    public void Clear()
    {

    }
}
