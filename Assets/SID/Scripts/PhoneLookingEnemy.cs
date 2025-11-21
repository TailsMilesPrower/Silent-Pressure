using UnityEngine;

public class PhoneLookingEnemy : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float downAngle = 45f;     // looking at phone angle
    public float upAngle = 0f;        // looking up angle
    public float rotationSpeed = 90f; // Rotation speed

    [Header("Timing Settings")]
    public float timeBetweenChecks = 5f; // Look at phone time
    public float lookUpDuration = 2f;    // Look up time

    // Original local rotation (Old version was useing the global rotation=> BAD)
    private Quaternion baseRotation;
    private void Start()
    {
        // Store the object’s starting orientation
        baseRotation = transform.localRotation;

        // Start tilted down
        transform.localRotation = baseRotation * Quaternion.Euler(downAngle, 0, 0);

        // Begin routine
        StartCoroutine(LookRoutine());
    }

    private System.Collections.IEnumerator LookRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenChecks);

            // Rotate up
            yield return StartCoroutine(RotateTo(upAngle));

            // Hold forward
            yield return new WaitForSeconds(lookUpDuration);

            // Rotate down
            yield return StartCoroutine(RotateTo(downAngle));
        }
    }

    private System.Collections.IEnumerator RotateTo(float targetAngle)
    {
        float currentAngle = 0f;
        // Compute current X rotation relative to base
        Quaternion currentRot = Quaternion.Inverse(baseRotation) * transform.localRotation;
        currentAngle = currentRot.eulerAngles.x;
        if (currentAngle > 180f) currentAngle -= 360f;

        while (Mathf.Abs(currentAngle - targetAngle) > 0.1f)
        {
            currentAngle = Mathf.MoveTowards(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
            transform.localRotation = baseRotation * Quaternion.Euler(currentAngle, 0, 0);
            yield return null;
        }
    }
}
