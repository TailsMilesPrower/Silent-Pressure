using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIRootConnector : MonoBehaviour
{
    public static UIRootConnector Instance;

    [Header("UI References (Auto Assigned)")]
    public StaminaUI staminaUI;
    public BreathingUI breathingUI;
    public StressMeter stressMeter;
    public ListenToMusic musicUI;

    [Header("Scene Change Handler")]
    public SceneChange sceneChange;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        //SceneManager.sceneLoaded += OnSceneLoaded;  
    }

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
        StartCoroutine(DelayBind());
    }

    private IEnumerator DelayBind()
    {
        yield return null;

        if (staminaUI == null) staminaUI = FindObjectOfType<StaminaUI>(true);
        if (breathingUI == null) breathingUI = FindObjectOfType<BreathingUI>(true);
        if (stressMeter == null) stressMeter = FindObjectOfType<StressMeter>(true);
        if (musicUI == null) musicUI = FindObjectOfType<ListenToMusic>(true);

        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player == null)
        {
            Debug.LogWarning("[UIRootConnector] No Player found in Scene");
            yield break;
        }

        if (staminaUI != null)
        {
            PlayerStamina ps = player.GetComponent<PlayerStamina>();
            staminaUI.playerStamina = ps;
        }

        BreathingMechanic breath = player.GetComponent<BreathingMechanic>();
        if (breath != null && breathingUI != null)
        {
            breath.breathingUI = breathingUI;

            foreach (SafeZoneTriggerRelay relay in FindObjectsOfType<SafeZoneTriggerRelay>())
            {
                relay.breathingMechanic = breath;
            }
        }

        if (musicUI != null && stressMeter != null)
        {
            musicUI.stressMeter = stressMeter;
        } 

        if (sceneChange != null)
        {
            sceneChange.player = player.transform;
        }

        Debug.Log("[UIRootConnector] UI successfully linked to new Player");
    }
}
