using UnityEngine;
using TMPro;

public class LevelButtonColor : MonoBehaviour
{
    [Header("این دکمه برای مرحله چندمه؟")]
    public int levelIndex = 1;

    [Header("رنگ‌ها")]
    public Color unlockedColor = Color.white;
    public Color lockedColor = new Color(0.3f, 0.3f, 0.3f);

    [Header("تکست مرحله (عدد مرحله)")]
    public TextMeshProUGUI levelText;

    private void Start()
    {
        UpdateState();
    }

    public void UpdateState()
    {
        int lastUnlocked = PlayerPrefs.GetInt("UnlockedLevel", 1);

        bool isUnlocked = levelIndex <= lastUnlocked;

        // تغییر رنگ متن
        if (levelText != null)
            levelText.color = isUnlocked ? unlockedColor : lockedColor;
    }
}
