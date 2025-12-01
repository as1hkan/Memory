using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Destroy(this.gameObject , 3.0f);
    }
}
