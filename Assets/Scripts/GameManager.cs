using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player Settings")]
    public SimpleGridMovement player;
    public LevelManager levelManager;
    public float rayDistance = 2f;
    public LayerMask blockMask;
    public LayerMask endMask;

    [Header("Blocks & UI")]
    public GameObject[] blocks;
    public GameObject[] blocksToFall;
    public TextMeshProUGUI countdownText;
    public Image goImage;
    public Image winImage;

    [HideInInspector] public bool gameEnded = false;
    private bool winRoutineStarted = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        int levelToLoad = PlayerPrefs.GetInt("SelectedLevel", 1);
        levelManager.Load(levelToLoad);

        StartCoroutine(StartTimer());
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

        // شمارش معکوس
        while (countdown > 0)
        {
            countdownText.text = Mathf.CeilToInt(countdown).ToString();
            yield return new WaitForSeconds(1f);
            countdown -= 1f;
        }

        // 🟢 در همین لحظه:
        // 1. متن پاک می‌شود
        // 2. تصویر "برو!" نمایش داده می‌شود
        // 3. بلاک‌ها غیب می‌شوند
        // 4. حرکت بازیکن فعال می‌شود
        countdownText.text = "";

        if (goImage != null)
            goImage.gameObject.SetActive(true);

        if (blocks != null && blocks.Length > 0)
        {
            foreach (GameObject block in blocks)
            {
                MeshRenderer renderer = block.GetComponent<MeshRenderer>();
                if (renderer != null)
                    renderer.enabled = false;
            }
        }

        if (player != null)
            player.canMove = true;

        // نمایش “برو!” برای ۱ ثانیه
        yield return new WaitForSeconds(1f);

        if (goImage != null)
            goImage.gameObject.SetActive(false);
    }

    // 🧱 نمایش بلاک زیر بازیکن
    void RevealBlockUnderPlayer(Vector3 playerPos)
    {
        Vector3 origin = playerPos + Vector3.up * 1f;
        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, rayDistance, blockMask))
        {
            MeshRenderer rend = hit.collider.GetComponent<MeshRenderer>();
            if (rend != null && !rend.enabled)
                rend.enabled = true;
        }
    }

    // 🏁 بررسی پایان
    void CheckWin(Vector3 playerPos)
    {
        Vector3 origin = playerPos + Vector3.up * 0.5f;
        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, rayDistance, endMask))
        {
            if (!winRoutineStarted)
            {
                winRoutineStarted = true;
                StartCoroutine(WaitForPlayerToFinishThenWin());
            }
        }
    }

    // 🕹️ انتظار تا پایان حرکت و افتادن بازیکن
    IEnumerator WaitForPlayerToFinishThenWin()
    {
        if (player != null)
        {
            while (player.IsMoving() || player.IsFalling())
                yield return null;
        }

        // قفل کردن حرکت بعد از برد
        if (player != null)
            player.canMove = false;

        gameEnded = true;
        StartCoroutine(HandleWin());
    }

    // 🎉 بردن مرحله
    IEnumerator HandleWin()
    {
        if (winImage != null)
            winImage.gameObject.SetActive(true);

        // انیمیشن افتادن بلاک‌ها
        if (blocksToFall != null && blocksToFall.Length > 0)
        {
            Vector3[] startPositions = new Vector3[blocksToFall.Length];
            for (int i = 0; i < blocksToFall.Length; i++)
                startPositions[i] = blocksToFall[i].transform.position;

            Vector3 moveOffset = Vector3.down * 40f;
            float fallDuration = 3f;
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

        if (winImage != null)
            winImage.gameObject.SetActive(false);

        // 🔹 تعیین مرحله فعلی
        string currentScene = SceneManager.GetActiveScene().name;
        int currentLevelNumber = 0;

        if (currentScene.StartsWith("LVL"))
        {
            string numberPart = currentScene.Replace("LVL", "").Trim();
            int.TryParse(numberPart, out currentLevelNumber);
        }

        // 🔓 باز کردن مرحله بعد
        int savedLevel = PlayerPrefs.GetInt("UnlockedLevel", -1);
        if (currentLevelNumber - 1 > savedLevel)
        {
            PlayerPrefs.SetInt("UnlockedLevel", currentLevelNumber - 1);
            PlayerPrefs.Save();
        }

        // 🔁 رفتن به مرحله بعد
        if (currentLevelNumber >= 40)
        {
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("End Scene");
        }
        else
        {
            int nextLevelNumber = currentLevelNumber + 1;
            string nextScene = "LVL " + nextLevelNumber;

            if (Application.CanStreamedLevelBeLoaded(nextScene))
                SceneManager.LoadScene(nextScene);
            else
                SceneManager.LoadScene("MenuScene");
        }
    }
}
