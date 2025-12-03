using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerReferenceResolver : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player == null) return;

        AssignPlayerToUI(player);
    }

    private void AssignPlayerToUI(PlayerController player)
    {
        if (FindAnyObjectByType<PhoneMenu>()) FindAnyObjectByType<PhoneMenu>().playerController = player;

        //if (FindAnyObjectByType<PlayerStamina>()) FindAnyObjectByType<StaminaUI>().gameObject;
        //if (FindAnyObjectByType<BreathingUI>()) FindAnyObjectByType<BreathingUI>().canvasGroup = player;
        //if (FindAnyObjectByType<StressMeter>()) FindAnyObjectByType<StressMeter>().needle = GetComponent<PlayerController>().gameObject;
    }
}
