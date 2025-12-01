using UnityEngine;

public class LangSwitch : MonoBehaviour
{
    public GameObject[] faBtns;
    public GameObject[] enBtns;

    bool isFarsi = true;

    public void ToggleLang()
    {
        isFarsi = !isFarsi;

        foreach (var b in faBtns)
            if (b != null) b.SetActive(isFarsi);

        foreach (var b in enBtns)
            if (b != null) b.SetActive(!isFarsi);
    }
}

