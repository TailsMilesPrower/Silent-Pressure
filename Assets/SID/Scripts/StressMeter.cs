using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Xasu.HighLevel;

public class StressMeter : MonoBehaviour
{
    [Header("Needle UI")]
    public RectTransform needle;

    // Anxiety values
    [Header("Settings")]
    [Range(0, 100)] public float anxiety = 0f;
    public float anxietyIncreaseRate = 10f;
    public float anxietyDecreaseRate = 15f;

    [Header("Needle Rotation")]
    public float minAngle = 90f;
    public float maxAngle = -90f;

    [Header("Stress States")]
    public bool stressing = false;  // Increase Stress
    public bool calming = false;    // Decrese Stress
    public bool stable = false;     // Maybe will use it for something later

    [Header("Game Over Settings")]
    public string gameOverSceneName = "GameOver";

    void Update()
    {
        // Toggles => ADD / REMOVE 

        // Toggles for Testing
        // Toggle stressing with F
        if (Keyboard.current.fKey.wasPressedThisFrame)
            stressing = !stressing;

        // Toggle calming with Space
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            calming = !calming;

        // Stress Logic
        // Stressing priority over calming
        // stressing = true & calming = true   => stress +
        // stressing = true & calming = false  => stress +
        // stressing = false & calming = true  => stress -
        // stressing = false & calming = false => no change.
        if (stressing)
        {
            anxiety += anxietyIncreaseRate * Time.deltaTime;
        }
        else if (calming)
        {
            anxiety -= anxietyDecreaseRate * Time.deltaTime;
        }

        anxiety = Mathf.Clamp(anxiety, 0f, 100f);

        // Needle Rotation
        float t = anxiety / 100f;
        float angle = Mathf.Lerp(minAngle, maxAngle, t);
        needle.localRotation = Quaternion.Euler(0, 0, angle);

        //Game Over
        if (anxiety >= 100f)
        {
            SceneManager.LoadScene(gameOverSceneName);
            CompletableTracker.Instance.Initialized("Death of Stress", CompletableTracker.CompletableType.Session);
        }
    }
}
