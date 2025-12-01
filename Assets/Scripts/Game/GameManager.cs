using UnityEngine;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player/Level")]
    public SimpleGridMovement player;
    public LevelManager levelManager;
    // public CameraMovement cameraMovement; // حذف شد: دیگر به دوربین کاری نداریم

    [Header("Ray Settings")]
    public float rayDistance = 2f;
    public LayerMask blockMask;
    public LayerMask endMask;

    [Header("Blocks")]
    public GameObject[] blocks;

    [Header("UI")]
    public TextMeshProUGUI countdownText;

    private bool gameEnded = false;
    private bool winRoutineStarted = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        int lvl = PlayerPrefs.GetInt("SelectedLevel", 1);

        levelManager.Load(lvl);

        // حذف شد: انیمیشن شروع دوربین
        // if (cameraMovement != null) cameraMovement.PlayStartAnimation();

        StartCoroutine(StartTimer());
    }

    private void Update()
    {
        if (!gameEnded && player.canMove)
        {
            RevealBlockUnderPlayer(player.transform.position);
            CheckWin(player.transform.position);
        }
    }

    // ================= COUNTDOWN =================
    IEnumerator StartTimer()
    {
        countdownText.text = "";

        float countdown = 3f;

        while (countdown > 0)
        {
            countdownText.text = Mathf.CeilToInt(countdown).ToString();
            StartCoroutine(FadeScale(countdownText));
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        foreach (var b in blocks)
        {
            if (b == null) continue;
            var r = b.GetComponent<MeshRenderer>();
            if (r != null) r.enabled = false;
        }

        countdownText.text = "برو!";
        StartCoroutine(FadeScale(countdownText));

        yield return new WaitForSeconds(0.6f);

        StartCoroutine(TextFadeOut(countdownText));
        yield return new WaitForSeconds(0.3f);

        countdownText.text = "";
        player.canMove = true;
    }

    // ================= BLOCK REVEAL =================
    void RevealBlockUnderPlayer(Vector3 pos)
    {
        if (Physics.Raycast(pos + Vector3.up * 0.5f, Vector3.down,
            out RaycastHit hit, rayDistance, blockMask))
        {
            var r = hit.collider.GetComponent<MeshRenderer>();
            if (!r.enabled) r.enabled = true;
        }
    }

    // ================= CHECK WIN =================
    void CheckWin(Vector3 pos)
    {
        if (Physics.Raycast(pos + Vector3.up * 0.2f, Vector3.down,
            out RaycastHit hit, rayDistance, endMask))
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

    // ================= HANDLE WIN =================
    IEnumerator HandleWin()
    {
        // حذف شد: زوم دوربین
        // if (cameraMovement != null) cameraMovement.ZoomIn();

        countdownText.text = "برنده شدی!";
        StartCoroutine(FadeScale(countdownText));

        yield return new WaitForSeconds(1.5f);
        
        StartCoroutine(TextFadeOut(countdownText));
        yield return new WaitForSeconds(0.5f);

        // *************** Save Progress & Next Level ***************
        int selected = PlayerPrefs.GetInt("SelectedLevel", 1);
        int unlocked = PlayerPrefs.GetInt("UnlockedLevel", 1);

        int next = selected + 1;
        int totalLevels = levelManager.levelsData.Count;

        // فقط اگر مرحله بعدی وجود دارد
        if (next <= totalLevels)
        {
            PlayerPrefs.SetInt("SelectedLevel", next);

            if (next > unlocked)
                PlayerPrefs.SetInt("UnlockedLevel", next);

            PlayerPrefs.Save();

            // Load next level
            levelManager.Load(next);

            // حذف شد: انیمیشن دوربین برای مرحله جدید
            // if (cameraMovement != null) cameraMovement.PlayStartAnimation();

            gameEnded = false;
            winRoutineStarted = false;

            StartCoroutine(StartTimer());
        }
        else
        {
            // 🚨 آخرین مرحله → برو End Scene
            SceneManager.LoadScene("End Scene");
        }
    }

    // ================= TEXT EFFECTS =================
    IEnumerator FadeScale(TextMeshProUGUI txt, float dur = .35f)
    {
        Color c = txt.color;
        txt.color = new Color(c.r, c.g, c.b, 0);

        Vector3 s = Vector3.one * 0.3f;
        Vector3 m = Vector3.one * 1.3f;
        Vector3 e = Vector3.one;

        txt.transform.localScale = s;

        float t = 0;
        while (t < dur)
        {
            t += Time.deltaTime;

            float p = t / dur;
            txt.color = new Color(c.r, c.g, c.b, p);

            if (p < .5f)
                txt.transform.localScale = Vector3.Lerp(s, m, p * 2f);
            else
                txt.transform.localScale = Vector3.Lerp(m, e, (p - .5f) * 2f);

            yield return null;
        }
    }

    IEnumerator TextFadeOut(TextMeshProUGUI txt, float dur = .3f)
    {
        Color c = txt.color;
        float t = 0;

        while (t < dur)
        {
            t += Time.deltaTime;
            float p = t / dur;

            txt.color = new Color(c.r, c.g, c.b, 1 - p);
            yield return null;
        }

        txt.color = new Color(c.r, c.g, c.b, 0);
    }
}