using UnityEngine;

public class FloatObject : MonoBehaviour
{
    [Header("Floating Settings")]
    public float amplitude = 0.5f;   // شدت حرکت
    public float frequency = 1f;     // سرعت حرکت

    public bool moveX = false;       // شناوری در محور X
    public bool moveY = true;        // شناوری در محور Y
    public bool moveZ = false;       // شناوری در محور Z

    private Vector3 startPos;

    void Start()
    {
        // ذخیره موقعیت اولیه
        startPos = transform.localPosition;
    }

    void Update()
    {
        // زمان برای تغییر نرم
        float offset = Mathf.Sin(Time.time * frequency) * amplitude;

        Vector3 newPos = startPos;

        if (moveX) newPos.x += offset;
        if (moveY) newPos.y += offset;
        if (moveZ) newPos.z += offset;

        transform.localPosition = newPos;
    }
}