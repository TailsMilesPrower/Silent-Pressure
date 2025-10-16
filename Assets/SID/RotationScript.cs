using UnityEngine;

public class RotationScript : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float speed = 45f;       // Rotation speed
    public float switchTime = 5f;   // Time before switching direction

    private float timer = 0f;
    private int direction = 1;

    void Update()
    {
        // Rotate
        transform.Rotate(Vector3.up * speed * direction * Time.deltaTime);

        // Timer
        timer = timer + Time.deltaTime;
        if (timer >= switchTime)
        {
            direction = direction * -1; // Reverse direction
            timer = 0f;
        }
    }
}
