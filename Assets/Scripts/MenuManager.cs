using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Level Buttons")]
    [SerializeField] private Button[] levelButtons;

    void Start()
    {
        // مقدار اولیه اگر وجود نداشت
        if (!PlayerPrefs.HasKey("UnlockedLevel"))
            PlayerPrefs.SetInt("UnlockedLevel", -1);

        int lastUnlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", -1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            // فقط مرحله‌های باز و مرحله‌ی بعدی فعال باشند
            if (i <= lastUnlockedLevel + 1)
                levelButtons[i].interactable = true;
            else
                levelButtons[i].interactable = false;

            // LVL 1 از Scene Index = 3 شروع می‌شود
            int sceneIndex = i + 3;

            levelButtons[i].onClick.RemoveAllListeners();
            int capturedIndex = sceneIndex;
            levelButtons[i].onClick.AddListener(() => LoadLevel(capturedIndex));
        }
    }

    // 🎮 دکمه Play → می‌برد به آخرین مرحله‌ی باز شده (نه جلوتر)
    public void PlayBtn()
    {
        int lastUnlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", -1);
        int nextLevelIndex = lastUnlockedLevel + 3; // دقیقاً خود مرحله‌ی بعدی

        // اگر هنوز هیچ مرحله‌ای باز نشده → برو به LVL 1
        if (nextLevelIndex < 3)
            nextLevelIndex = 3;

        SceneManager.LoadScene(nextLevelIndex);
    }

    public void LevelsBtn()
    {
        SceneManager.LoadScene(2); // لیست مراحل
    }

    public void MainMenuBtn()
    {
        SceneManager.LoadScene(0);
    }

    public void AboutMe()
    {
        SceneManager.LoadScene(1);
    }

    public void Back_Menu()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitBtn()
    {
        Application.Quit();
    }

    public void LoadLevel(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
