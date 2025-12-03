using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class SceneChangeCollider : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Name of the scene to load. Leave empty to use build index.")]
    [SerializeField] private string sceneName;
    [Tooltip("Name of the return scene to load")]
    [SerializeField] private string returnSceneName = "Town";

    //public int sceneBuildIndex = -1;

    [Header("Trigger Settings")]
    [SerializeField] private string targetTag = "Player";
    [SerializeField] private float disableDurationOnReturn = 60f;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnOffset = 1.5f;
    [SerializeField] private float heightOffset = 0.5f;

    //[Header("Time of Day Settings")] // Disabled Mechanic (Scrapped)
    //[SerializeField] private Light directionalLight;
    //[SerializeField] private float sunRotationPerMinute = 30f;

    [Header("Fade Transition Settings")]
    [SerializeField] private CanvasGroup fadeCanvas;
    [SerializeField] private float fadeDuration = 1.0f;

    [Header("Saving")]
    [SerializeField] private bool disableSavingInThisScene = false;

    private static Vector3 savedSpawnPosition;
    private static Quaternion savedSpawnRotation;
    private static bool spawnPending = false;
    //private static string intendedReturnScene = "";
    //private static bool comingFromLevel = false;
    private static float suppressedUntil = 0f;

    private Collider triggerCollider;
    private static float levelStartTime;

    /*
    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }
    */


    private void Awake()
    {
        triggerCollider = GetComponent<Collider>();
        triggerCollider.isTrigger = true;

        float now = Time.realtimeSinceStartup;
        if (now < suppressedUntil)
        {
            triggerCollider.enabled = false;
            StartCoroutine(ReenableWhenSuppressedEnds(suppressedUntil - now));
        }

        //if (SceneManager.GetActiveScene().name == lastSceneName) return;
        //if (SceneManager.GetActiveScene().name != lastSceneName && hasSavedSpawn) StartCoroutine(DisableColliderTemporarily());
        //if (!triggerCollider.isTrigger) triggerCollider.isTrigger = true;
    }

    private IEnumerator ReenableWhenSuppressedEnds(float seconds)
    {
        if (seconds > 0f) yield return new WaitForSeconds(seconds);
        if (triggerCollider != null) triggerCollider.enabled = true;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        levelStartTime = Time.time;

        //previously comingFromLevel was here in the if statement
        if (spawnPending && SceneManager.GetActiveScene().name == returnSceneName)
        {
            MovePlayerToSavedSpawn();
            
            if (ObjectiveManager.Instance != null && ObjectiveManager.Instance.HasNewObjective)
            {
                NotificationManager.Instance.TriggerNotification("You got mail - check your phone.");
                ObjectiveManager.Instance.AssignRandomObjective();
            }
            
            spawnPending = false;

            float now = Time.realtimeSinceStartup;
            suppressedUntil = Mathf.Max(suppressedUntil, now + disableDurationOnReturn);

            if (triggerCollider != null && suppressedUntil > now)
            {
                triggerCollider.enabled = false;
                StartCoroutine(ReenableWhenSuppressedEnds(suppressedUntil - now));
            }

            /*
            GameObject player = GameObject.FindGameObjectWithTag(targetTag);
            if (player != null)
            {
                player.transform.position = savedSpawnPosition;
                player.transform.rotation = savedSpawnRotation;
            }
            */

            //StartCoroutine(DisableColliderTemporarily());
            
            //comingFromLevel = false;
        }

        if (fadeCanvas)
        {
            fadeCanvas.gameObject.SetActive(true);
            StartCoroutine(FadeIn());
        }

        //lastSceneName = SceneManager.GetActiveScene().name;

        /*
        // The old IEnumerator method
        if (fadeCanvas)
        {
            fadeCanvas.gameObject.SetActive(true);
            fadeCanvas.alpha = 1f;
            yield return StartCoroutine(Fade(0f));
            fadeCanvas.blocksRaycasts = false;
        }
        */

        //Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;
    }

    private void MovePlayerToSavedSpawn()
    {
        GameObject player = GameObject.FindGameObjectWithTag(targetTag);

        if (player == null)
        {
            Debug.LogWarning($"[SceneChangeCollider] MovePlayerToSavedSpawn: Player not found with tag" + targetTag);
            return;
        }

        var cc = player.GetComponent<CharacterController>();
        if (cc) cc.enabled = false;

        player.transform.SetPositionAndRotation(savedSpawnPosition, savedSpawnRotation);

        if (cc) cc.enabled = true;

        Debug.Log($"[SceneChangeCollider] Player moved to saved spawn: " + savedSpawnPosition);

        /*
        if (player != null)
        {
            CharacterController controller = player.GetComponent<CharacterController>();
            if (controller) controller.enabled = false;

            player.transform.SetPositionAndRotation(savedSpawnPosition, savedSpawnRotation);

            if (controller) controller.enabled = true;
        }
        else
        {
            Debug.LogWarning("Player not found when trying to move to spawn position!");
        }
        */
    }

    /*
    private IEnumerator DisableColliderTemporarily()
    {
        triggerCollider.enabled = false;
        yield return new WaitForSeconds(disableDurationOnReturn);
        triggerCollider.enabled = true;
    }
    */

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(targetTag)) return;

        string currentScene = SceneManager.GetActiveScene().name;

        //bool skipSavingBecausePending = spawnPending;
        bool isColliderInReturnScene = currentScene == returnSceneName;

        /*
        if (currentScene == returnSceneName)
        {
            Debug.Log($"[SceneChangeCollider] Ignored trigger in return scene ({currentScene}) to prevent overwrite.");
            return;
        }
        */

        if (!string.IsNullOrEmpty(sceneName))
        {
            if (fadeCanvas) 
                StartCoroutine(FadeAndLoad(sceneName));
            else
                SceneManager.LoadScene(sceneName);
        }
  
        /*
        else if (sceneBuildIndex >= 0)
        {
            SceneManager.LoadScene(sceneBuildIndex);
        }
        */

        else
        {
            Debug.LogWarning($"{name}: No valid scene assigned to load!");
        }

        //savedSpawnPosition = transform.position + transform.forward * spawnOffset;
        //savedSpawnRotation = Quaternion.LookRotation(-transform.forward, Vector3.up);

        /* //Old version of the save position and rotation
        if (!skipSavingBecausePending)
        {
            string activeScene = SceneManager.GetActiveScene().name;
            
            if (activeScene == "Home")
            {
                Debug.Log("[SceneChangeCollider] Skipping spawn save in Home");
            }
            else
            {
                Vector3 forward = transform.forward;
                Collider myCollider = GetComponent<Collider>();

                Vector3 frontPoint = myCollider.ClosestPoint(transform.position + forward * 10f);
                Vector3 spawnPos = frontPoint + forward * spawnOffset + Vector3.up * heightOffset;
                Quaternion spawnRot = Quaternion.LookRotation(forward, Vector3.up);

                savedSpawnPosition = spawnPos;
                savedSpawnRotation = spawnRot;
                spawnPending = true;
                intendedReturnScene = returnSceneName;

                Debug.Log($"[SceneChangeCollider] Saved spawn pos: {spawnPos} | Collider at: {transform.position}");
            }
        }
        else
        {
            Debug.Log($"[SceneChangeCollider] Spawn already pending - not overwriting saved spawn. Collider at {transform.position}");
        }
        */

        if (!disableSavingInThisScene && !isColliderInReturnScene && !spawnPending)
        {
            Vector3 forward = transform.forward;
            Collider myCollider = GetComponent<Collider>();

            Vector3 frontPoint = myCollider.ClosestPoint(transform.position + forward * 10f);
            Vector3 spawnPos = frontPoint + forward * spawnOffset + Vector3.up * heightOffset;
            Quaternion spawnRot = Quaternion.LookRotation(forward, Vector3.up);

            savedSpawnPosition = spawnPos;
            savedSpawnRotation = spawnRot;
            spawnPending = true;
            
            Debug.Log($"[SceneChangeCollider] Saved spawn pos: {spawnPos} | Collider at: {transform.position}");
        }
        else
        {
            Debug.Log($"[SceneChangeCollider] Not saving spawn here (scene:{currentScene})");
        }


        float timeSpent = Time.time - levelStartTime;

        /* //Disabled Time of Day (Scrapped)
        if (directionalLight && SceneManager.GetActiveScene().name != returnSceneName)
        {
            float rotationChange = Mathf.Clamp((timeSpent / 60f) * sunRotationPerMinute, 0f, 90f);
            directionalLight.transform.Rotate(Vector3.right * rotationChange, Space.Self);
        }
        */

        float now = Time.realtimeSinceStartup;
        suppressedUntil = Mathf.Max(suppressedUntil, now + disableDurationOnReturn);

        //comingFromLevel = SceneManager.GetActiveScene().name != returnSceneName;

        if (fadeCanvas)
        {
            StartCoroutine(FadeAndLoad(sceneName));
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }

    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        if (fadeCanvas)
        {
            fadeCanvas.blocksRaycasts = true;

            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                fadeCanvas.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
                yield return null;
            }

            //yield return StartCoroutine(Fade(1f));
            fadeCanvas.alpha = 0f;
        }

        SceneManager.LoadScene(sceneName);

    }


    private IEnumerator FadeIn()
    {
        if (fadeCanvas == null) yield break;

        fadeCanvas.alpha = 1f;
        fadeCanvas.blocksRaycasts = true;
        
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }
        
        fadeCanvas.alpha = 0f;
        fadeCanvas.blocksRaycasts = false;
    }


    /*
    private IEnumerator Fade(float targetAlpha)
    {
        if (fadeCanvas == null) yield break;

        float startAlpha = fadeCanvas.alpha;
        float t = 0f;
        
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(startAlpha, targetAlpha, t / fadeDuration);
            yield return null;
        }
        fadeCanvas.alpha = targetAlpha;
    }
    */

}
