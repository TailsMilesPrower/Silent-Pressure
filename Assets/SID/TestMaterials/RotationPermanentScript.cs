using UnityEngine;

public class RotationPermanentScript : MonoBehaviour
{
    [Header("Rotation Speed")]
    public float rotationSpeed = 50f;

    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
