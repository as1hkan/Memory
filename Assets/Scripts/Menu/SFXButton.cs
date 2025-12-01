using UnityEngine;

public class SFXButton : MonoBehaviour
{
    public void OnClick()
    {
        MusicManager.instance.OnMuteSFXButtonClick();
    }
}
