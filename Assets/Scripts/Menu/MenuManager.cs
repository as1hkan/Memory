using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("Level Buttons")]
    [SerializeField] private Button[] levelButtons;

    [Header("Level Text Colors")]
    [SerializeField] private Color unlockedColor = Color.white;
    [SerializeField] private Color lockedColor = new Color(0.4f, 0.4f, 0.4f);

    void Start()
    {
        // اگر هیچ لولی ذخیره نشده، از لول 1 شروع کن
        if (!PlayerPrefs.HasKey("UnlockedLevel"))
            PlayerPrefs.SetInt("UnlockedLevel", 1);

        int lastUnlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelIndex = i + 1;  // Level number (1,2,3,...)

            bool isUnlocked = levelIndex <= lastUnlockedLevel;

            // فعال بودن دکمه
            levelButtons[i].interactable = isUnlocked;

            // رنگ متن دکمه
            TextMeshProUGUI txt = levelButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (txt != null)
                txt.color = isUnlocked ? unlockedColor : lockedColor;

            // کلیک
            levelButtons[i].onClick.RemoveAllListeners();
            int capturedLevel = levelIndex;

            levelButtons[i].onClick.AddListener(() =>
            {
                SelectLevel(capturedLevel);
            });
        }
    }

    void SelectLevel(int levelIndex)
    {
        PlayerPrefs.SetInt("SelectedLevel", levelIndex);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Game");
    }

    // دکمه Play
    public void PlayBtn()
    {
        int lastUnlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        int levelToPlay = Mathf.Clamp(lastUnlockedLevel, 1, levelButtons.Length);

        PlayerPrefs.SetInt("SelectedLevel", levelToPlay);
        PlayerPrefs.Save();

        SceneManager.LoadScene("Game");
    }

    // دکمه Levels
    public void LevelsBtn()
    {
        SceneManager.LoadScene("Levels");
    }

    // برگشت به منوی اصلی
    public void MainMenuBtn()
    {
        SceneManager.LoadScene("Menu");
    }

    // صفحه درباره من
    public void AboutMe()
    {
        SceneManager.LoadScene("About Me");
    }

    // خروج
    public void ExitBtn()
    {
        Application.Quit();
    }
}
