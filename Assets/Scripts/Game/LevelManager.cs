using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform camera;
    [SerializeField] private SimpleGridMovement player;
    [SerializeField] public List<Level> levelsData;   
    [SerializeField] private Transform parent;

    [Header("Prefabs")]
    [SerializeField] private GameObject CubePrefab;
    [SerializeField] private GameObject CubeStart;
    [SerializeField] private GameObject CubeEnd;

    private GameObject currentLevelGO;

    public void Load(int index)
    {
      
        if (levelsData == null || levelsData.Count == 0)
        {
            Debug.LogError("LevelManager: levelsData is empty!");
            return;
        }

        index = Mathf.Clamp(index, 1, levelsData.Count);

        Debug.Log("LevelManager Loading = " + index);

        if (currentLevelGO != null)
            Destroy(currentLevelGO);

        currentLevelGO = new GameObject("LEVEL_" + index);
        currentLevelGO.transform.SetParent(parent);

        Level lvl = levelsData[index - 1];

        List<GameObject> blocks = new List<GameObject>();
     

        // START BLOCK
        GameObject startObj = Instantiate(
            CubeStart,
            new Vector3(lvl.StartPos.x, 0, lvl.StartPos.y),
            Quaternion.identity,
            currentLevelGO.transform
        );
      

        // PATH
        foreach (var pos in lvl.Cubes)
        {
            GameObject cube = Instantiate(
                CubePrefab,
                new Vector3(pos.x, 0, pos.y),
                Quaternion.identity,
                currentLevelGO.transform
            );

            blocks.Add(cube);
        }

        // END BLOCK
        GameObject endObj = Instantiate(
            CubeEnd,
            new Vector3(lvl.EndPos.x, 0, lvl.EndPos.y),
            Quaternion.identity,
            currentLevelGO.transform
        );
       

        // COLOR GRADIENT
        for (int i = 0; i < blocks.Count; i++)
        {
            float t = (float)i / Mathf.Max(1, blocks.Count - 1);
            Color color = Color.Lerp(lvl.StartColor, lvl.EndColor, t);

            blocks[i].GetComponent<MeshRenderer>().material.color = color;
          
        }

        
        player.transform.position = new Vector3(lvl.StartPos.x, 1f, lvl.StartPos.y);
        player.canMove = false;
        player.isWinning = false;

        camera.position = lvl.CameraPosition;

        GameManager.instance.blocks = blocks.ToArray();

        

        Debug.Log("Loaded Level " + index);
    }
}