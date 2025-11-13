using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player/Level")]
    public SimpleGridMovement player;
    public LevelManager levelManager;

    [Header("Ray Settings")]
    public float rayDistance = 2f;
    public LayerMask blockMask;
    public LayerMask endMask;

    [Header("Blocks")]
    public GameObject[] blocks;
    public GameObject[] blocksToFall;
    public Transform StartPoint;
    public Transform EndPoint;

    [Header("UI")]
    public TextMeshProUGUI countdownText;
    public Image goImage;
    public Image winImage;

    private bool gameEnded = false;
    private bool winRoutineStarted = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        int lvl = PlayerPrefs.GetInt("SelectedLevel", 1);
        levelManager.Load(lvl);

        StartCoroutine(StartTimer());
    }

    void Update()
    {
        if (!gameEnded && player.canMove)
        {
            RevealBlockUnderPlayer(player.transform.position);
            CheckWin(player.transform.position);
        }
    }

    IEnumerator StartTimer()
    {
        float countdown = 3f;

        while (countdown > 0)
        {
            countdownText.text = Mathf.CeilToInt(countdown).ToString();
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        countdownText.text = "";
        goImage.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.7f);
        goImage.gameObject.SetActive(false);

        // Hide path blocks until stepped on
        foreach (var block in blocks)
        {
            var r = block.GetComponent<MeshRenderer>();
            if (r != null) r.enabled = false;
        }

        player.canMove = true;
    }

    void RevealBlockUnderPlayer(Vector3 pos)
    {
        Vector3 origin = pos + Vector3.up * 0.5f;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, rayDistance, blockMask))
        {
            MeshRenderer r = hit.collider.GetComponent<MeshRenderer>();
            if (r != null && !r.enabled)
                r.enabled = true;
        }
    }

    void CheckWin(Vector3 pos)
    {
        Vector3 origin = pos + Vector3.up * 0.2f;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, rayDistance, endMask))
        {
            if (!winRoutineStarted)
            {
                winRoutineStarted = true;
                StartCoroutine(WaitForStopThenWin());
            }
        }
    }

    IEnumerator WaitForStopThenWin()
    {
        while (player.IsMoving() || player.IsFalling())
            yield return null;

        player.canMove = false;
        player.isWinning = true;
        gameEnded = true;

        StartCoroutine(HandleWin());
    }

    IEnumerator HandleWin()
    {
        winImage.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        // Sort by StartPoint
        blocksToFall = blocksToFall
            .OrderBy(b => Vector3.Distance(b.transform.position, StartPoint.position))
            .ToArray();

        float fallTime = 2f;
        float fallDistance = 40f;
        float delay = 0.18f;

        bool playerFallStarted = false;

        for (int i = 0; i < blocksToFall.Length; i++)
        {
            Transform block = blocksToFall[i].transform;

            Vector3 start = block.position;
            Vector3 end = start + Vector3.down * fallDistance;

            StartCoroutine(FallBlock(block, start, end, fallTime));

            if (!playerFallStarted && block == EndPoint)
            {
                playerFallStarted = true;
                StartCoroutine(FallPlayer(player.transform, block, fallTime, fallDistance));
            }

            yield return new WaitForSeconds(delay);
        }

        yield return new WaitForSeconds(1.8f);

        int current = PlayerPrefs.GetInt("SelectedLevel", 1);
        int next = current + 1;

        PlayerPrefs.SetInt("SelectedLevel", next);
        PlayerPrefs.Save();

        yield return StartCoroutine(FadeAndLoad(next));
    }

    IEnumerator FallBlock(Transform t, Vector3 start, Vector3 end, float dur)
    {
        float time = 0;

        while (time < dur)
        {
            time += Time.deltaTime;
            float p = Mathf.SmoothStep(0, 1, time / dur);
            t.position = Vector3.Lerp(start, end, p);
            yield return null;
        }
    }

    IEnumerator FallPlayer(Transform player, Transform endPoint, float dur, float dist)
    {
        float time = 0;

        Vector3 start = player.position;
        Vector3 end = start + Vector3.down * dist;

        while (time < dur)
        {
            time += Time.deltaTime;
            float p = Mathf.SmoothStep(0, 1, time / dur);

            Vector3 ep = endPoint.position;
            player.position = new Vector3(ep.x, Mathf.Lerp(start.y, end.y, p), ep.z);

            yield return null;
        }
    }

    IEnumerator FadeAndLoad(int nextLevel)
    {
        GameObject fadeObj = new GameObject("FadePanel");
        Canvas canvas = fadeObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;

        CanvasGroup cg = fadeObj.AddComponent<CanvasGroup>();
        fadeObj.AddComponent<GraphicRaycaster>();

        Image img = fadeObj.AddComponent<Image>();
        img.color = Color.black;
        cg.alpha = 0f;

        float t = 0;
        float dur = 0.8f;

        while (t < dur)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(0, 1, t / dur);
            yield return null;
        }

        levelManager.Load(nextLevel);
    }
}
