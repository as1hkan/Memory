using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SimpleGridMovement : MonoBehaviour
{
    
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float moveDistance = 1f;
    public float fallThreshold = -5f;
    public bool canMove = false;

    private bool isMoving = false;
    private bool isFalling = false;
    private Vector3 moveDir;
    private Vector3 pivot;

    [Header("Touch Settings")]
    [SerializeField] private float swipeThreshold = 50f;
    [SerializeField] private float sens = 10f;
    private Vector2 touchStartPos;
    private bool touchStarted;

    [Header("Sound Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip moveClip;
    [SerializeField] private AudioClip loseClip;   // 🔹 صدای باخت

    public bool isWinning = false;
    void Start()
    {
        StartCoroutine(StartTimer());

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f;
        }
    }

    void Update()
    {
        // 🔹 بررسی سقوط
        if (!isFalling && !IsBlockUnderPlayer())
        {
            StartCoroutine(FallDown());
            return;
        }

        if (isFalling || isMoving) return;

        moveDir = Vector3.zero;

        // ⌨️ کنترل با کیبورد
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.RightArrow))
            moveDir = Vector3.right;
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.LeftArrow))
            moveDir = Vector3.left;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.UpArrow))
            moveDir = Vector3.forward;
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.DownArrow))
            moveDir = Vector3.back;

        // 📱 کنترل با لمس
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = touch.position;
                    touchStarted = true;
                    break;

                case TouchPhase.Ended:
                    if (!touchStarted) break;

                    Vector2 delta = touch.position - touchStartPos;
                    touchStarted = false;

                    if (delta.magnitude < swipeThreshold) return;

                    float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
                    angle = (angle + 360f) % 360f;

                    if (angle >= (45 - sens) && angle < (45 + sens))
                        moveDir = Vector3.forward;
                    else if (angle >= (135 - sens) && angle < (135 + sens))
                        moveDir = Vector3.left;
                    else if (angle >= (225 - sens) && angle < (225 + sens))
                        moveDir = Vector3.back;
                    else if (angle >= (315 - sens) && angle < (315 + sens))
                        moveDir = Vector3.right;

                    break;
            }
        }

        // 🚀 شروع حرکت
        if (moveDir != Vector3.zero && canMove)
        {
            StartCoroutine(Roll(moveDir));
        }
    }

    IEnumerator Roll(Vector3 direction)
    {
        isMoving = true;
        PlayMoveSound();

        Vector3 axis = Vector3.Cross(Vector3.up, direction);
        pivot = transform.position + (Vector3.down * 0.5f) + (direction * 0.5f);

        float angle = 0;
        while (angle < 90)
        {
            float step = moveSpeed * Time.deltaTime * 90;
            float rotation = Mathf.Min(step, 90 - angle);

            transform.RotateAround(pivot, axis, rotation);
            angle += rotation;

            yield return null;
        }

        transform.position = new Vector3(
            Mathf.Round(transform.position.x),
            Mathf.Round(transform.position.y),
            Mathf.Round(transform.position.z)
        );

        transform.rotation = Quaternion.Euler(
            Mathf.Round(transform.rotation.eulerAngles.x / 90) * 90,
            Mathf.Round(transform.rotation.eulerAngles.y / 90) * 90,
            Mathf.Round(transform.rotation.eulerAngles.z / 90) * 90
        );

        isMoving = false;
    }

    bool IsBlockUnderPlayer()
    {
        Vector3 origin = transform.position + Vector3.up * 0.5f;
        return Physics.Raycast(origin, Vector3.down, out _, 2f);
    }

    IEnumerator FallDown()
    {
        isFalling = true;
        isMoving = true;

        // 🔊 پخش صدای باخت
        if (audioSource != null && loseClip != null)
            audioSource.PlayOneShot(loseClip);

        float fallSpeed = 0f;
        float gravity = 20f;   // می‌تونی زیادترش کنی برای سرعت بیشتر

        while (transform.position.y > fallThreshold)
        {
            fallSpeed += gravity * Time.deltaTime;
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(
            Mathf.Round(transform.position.x),
            Mathf.Round(transform.position.y),
            Mathf.Round(transform.position.z)
        );

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(5f);
        canMove = true;
    }

    private void PlayMoveSound()
    {
        if (audioSource != null && moveClip != null)
            audioSource.PlayOneShot(moveClip);
    }

    // ✅ متدهای عمومی برای اطلاع GameManager
    public bool IsMoving() { return isMoving; }
    public bool IsFalling() { return isFalling; }
}
