using UnityEngine;

public class MusicButton : MonoBehaviour
{
    public void OnClick()
    {
        MusicManager.instance.OnMuteMusicButtonClick();
    }
}
