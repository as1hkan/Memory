using UnityEngine;

public class PlayerFall : MonoBehaviour
{
    private Vector3 startPosition;
    private bool shouldFall = false;
    private Vector3 moveOffset = Vector3.down * 3f;
    private float fallDuration = 1f;
    private float elapsed = 0f;

    void Update()
    {
        if (shouldFall)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fallDuration;
            transform.position = Vector3.Lerp(startPosition, startPosition + moveOffset, t);

            if (elapsed >= fallDuration)
            {
                shouldFall = false;
            }
        }
    }

    public void StartFalling()
    {
        startPosition = transform.position;
        shouldFall = true;
        elapsed = 0f;
    }
}