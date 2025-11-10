using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// A single-scene variant of your GameManager.
/// Keeps your original gameplay logic (countdown, reveal, win), but
/// delegates "load next level" to LevelManager instead of SceneManager.
/// </summary>
public class SingleSceneGameManager : MonoBehaviour
{
    [Header("Injected at runtime by LevelManager.BindRuntime")]
    public SimpleGridMovement player;
    public LayerMask blockMask;
    public LayerMask endMask;
    public GameObject[] blocks;
    public GameObject[] blocksToFall;

    [Header("UI")]
    [Tooltip("برای شمارش معکوس اعداد (۵ تا ۱) استفاده می‌شود")]
    public TextMeshProUGUI countdownText;

    [Tooltip("عکس PNG برای نمایش 'برو!'")]
    public Image goImage;

    [Tooltip("عکس PNG برای نمایش 'بردی!'")]
    public Image winImage;

    [Header("Wiring")]
    public LevelManager levelManager;

    [HideInInspector] public bool gameEnded = false;
    private bool winRoutineStarted = false;
    private bool started = false;

    void Start()
    {
        // Will start countdown as soon as LevelManager binds the runtime refs.
        // If it has already bound, Begin() will be called below.
    }

    /// <summary>
    /// Called by LevelManager after it spawns a level. This keeps StartTimer logic intact.
    /// </summary>
    public void BindRuntime(GameObject[] blocks, GameObject[] blocksToFall, SimpleGridMovement player, LayerMask blockMask, LayerMask endMask, int levelIndex)
    {
        this.blocks = blocks;
        this.blocksToFall = blocksToFall;
        this.player = player;
        this.blockMask = blockMask;
        this.endMask = endMask;

        gameEnded = false;
        winRoutineStarted = false;

        // Reset player
        if (player != null)
        {
            player.canMove = false;
            // Snap rotation/position rounding safety
            player.transform.rotation = Quaternion.identity;
        }

        // Start the usual countdown flow
        if (!started)
        {
            started = true;
            StartCoroutine(StartTimer());
        }
        else
        {
            // Subsequent levels: restart the timer each time.
            StopAllCoroutines();
            StartCoroutine(StartTimer());
        }
    }

    void Update()
    {
        if (player != null && player.canMove && !gameEnded)
        {
            RevealBlockUnderPlayer(player.transform.position);
            CheckWin(player.transform.position);
        }
    }

    // 🕒 شمارش معکوس و شروع بازی
    IEnumerator StartTimer()
    {
        float countdown = 5f;

        while (countdown > 0)
        {
            if (countdownText != null)
                countdownText.text = Mathf.CeilToInt(countdown).ToString();

            yield return new WaitForSeconds(1f);
            countdown -= 1f;
        }

        // پاک کردن متن
        if (countdownText != null) countdownText.text = "";

        // Go!
        if (goImage != null) goImage.gameObject.SetActive(true);

        // مخفی کردن همه‌ی بلاک‌ها (مثل نسخه‌ی صحنه‌ای)
        if (blocks != null && blocks.Length > 0)
        {
            foreach (GameObject block in blocks)
            {
                var renderer = block.GetComponent<MeshRenderer>();
                if (renderer != null) renderer.enabled = false;
            }
        }

        // فعال شدن حرکت بازیکن
        if (player != null) player.canMove = true;

        yield return new WaitForSeconds(1f);
        if (goImage != null) goImage.gameObject.SetActive(false);
    }

    // 🧱 نمایش بلاک زیر بازیکن
    void RevealBlockUnderPlayer(Vector3 playerPos)
    {
        Vector3 origin = playerPos + Vector3.up * 1f;
        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 2f, blockMask))
        {
            var rend = hit.collider.GetComponent<MeshRenderer>();
            if (rend != null && !rend.enabled)
                rend.enabled = true;
        }
    }

    // 🏁 بررسی پایان
    void CheckWin(Vector3 playerPos)
    {
        Vector3 origin = playerPos + Vector3.up * 0.5f;
        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 2f, endMask))
        {
            if (!winRoutineStarted)
            {
                winRoutineStarted = true;
                StartCoroutine(WaitForPlayerToFinishThenWin());
            }
        }
    }

    IEnumerator WaitForPlayerToFinishThenWin()
    {
        if (player != null)
        {
            while (player.IsMoving() || player.IsFalling())
                yield return null;
        }

        if (player != null) player.canMove = false;

        gameEnded = true;
        StartCoroutine(HandleWin());
    }

    IEnumerator HandleWin()
    {
        if (winImage != null) winImage.gameObject.SetActive(true);

        // انیمیشن افتادن بلاک‌ها (اختیاری)
        if (blocksToFall != null && blocksToFall.Length > 0)
        {
            Vector3[] startPositions = new Vector3[blocksToFall.Length];
            for (int i = 0; i < blocksToFall.Length; i++)
                startPositions[i] = blocksToFall[i].transform.position;

            Vector3 moveOffset = Vector3.down * 20f;
            float fallDuration = 2f;
            float elapsed = 0f;

            while (elapsed < fallDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / fallDuration;
                for (int i = 0; i < blocksToFall.Length; i++)
                    blocksToFall[i].transform.position =
                        Vector3.Lerp(startPositions[i], startPositions[i] + moveOffset, t);
                yield return null;
            }
        }

        yield return new WaitForSeconds(0.8f);
        if (winImage != null) winImage.gameObject.SetActive(false);

        // 🔓 باز کردن مرحله بعد (PlayerPrefs همان منطق قبلی، اما با ایندکس)
        int currentSaved = PlayerPrefs.GetInt("UnlockedLevel", -1);
        int currentIndex = levelManager != null ? levelManager.GetCurrentIndex() : 0;

        if (currentIndex > currentSaved)
        {
            PlayerPrefs.SetInt("UnlockedLevel", currentIndex);
            PlayerPrefs.Save();
        }

        // 🔁 رفتن به مرحله بعد (بدون Scene)
        if (levelManager != null)
        {
            levelManager.LoadNextLevel();
        }
    }
}
