using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    public LevelManager levelManager;
    public Button nextButton;
    public Button prevButton;

    void Start()
    {
        if (nextButton != null) nextButton.onClick.AddListener(() => levelManager.LoadNextLevel());
        if (prevButton != null) prevButton.onClick.AddListener(() => levelManager.LoadPrevLevel());
    }
}
