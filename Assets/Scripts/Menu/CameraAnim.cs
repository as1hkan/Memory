using UnityEngine;

public class CameraAnim : MonoBehaviour
{
    [SerializeField] private Vector3[] positions;
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private int status = 0;

    void LateUpdate()
    {
        if (positions.Length == 0) return;

        Vector3 targetPos = positions[status];

        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            smoothSpeed * Time.deltaTime
        );
    }

    public void PlusStatus()
    {
        if (status < positions.Length - 1)
            status++;
    }

    public void ManfiStatus()
    {
        if (status > 0)
            status--;
    }
    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 90;
    }
}
