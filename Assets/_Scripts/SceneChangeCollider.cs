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

    [Header("Time of Day Settings")]
    [SerializeField] private Light directionalLight;
    [SerializeField] private float sunRotationPerMinute = 30f;

    [Header("Fade Transition Settings")]
    [SerializeField] private CanvasGroup fadeCanvas;
    [SerializeField] private float fadeDuration = 1.0f;

    private static Vector3 savedSpawnPosition;
    private static Quaternion savedSpawnRotation;
    private static bool hasSavedSpawn = false;
    private static string lastSceneName = "";
    //private static bool comingFromLevel = false;
    private static float levelStartTime;

    private Collider triggerCollider;

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

        if (SceneManager.GetActiveScene().name == lastSceneName) return;
        if (SceneManager.GetActiveScene().name != lastSceneName && hasSavedSpawn) StartCoroutine(DisableColliderTemporarily());
        //if (!triggerCollider.isTrigger) triggerCollider.isTrigger = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        levelStartTime = Time.time;

        //previously comingFromLevel was here in the if statement
        if (hasSavedSpawn && SceneManager.GetActiveScene().name == returnSceneName)
        {
            GameObject player = GameObject.FindGameObjectWithTag(targetTag);
            if (player != null)
            {
                player.transform.position = savedSpawnPosition;
                player.transform.rotation = savedSpawnRotation;
            }
            
            //StartCoroutine(DisableColliderTemporarily());
            //MovePlayerToSavedSpawn();
            //comingFromLevel = false;
        }

        if (fadeCanvas) StartCoroutine(FadeIn());
        lastSceneName = SceneManager.GetActiveScene().name;

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


    private IEnumerator DisableColliderTemporarily()
    {
        triggerCollider.enabled = false;
        yield return new WaitForSeconds(disableDurationOnReturn);
        triggerCollider.enabled = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(targetTag)) return;

        /*
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        */
        /*
        else if (sceneBuildIndex >= 0)
        {
            SceneManager.LoadScene(sceneBuildIndex);
        }
        */
        /*
        else
        {
            Debug.LogWarning($"{name}: No valid scene assigned to load!");
        }
        */

        //savedSpawnPosition = transform.position + transform.forward * spawnOffset;
        //savedSpawnRotation = Quaternion.LookRotation(-transform.forward, Vector3.up);

        Vector3 forward = transform.forward;
        Collider myCollider = GetComponent<Collider>();

        Vector3 frontPoint = myCollider.ClosestPoint(transform.position + forward * 10f);
        Vector3 spawnPos = frontPoint + forward * spawnOffset + Vector3.up * heightOffset;
        Quaternion spawnRot = Quaternion.LookRotation(-forward, Vector3.up);

        savedSpawnPosition = spawnPos;
        savedSpawnRotation = spawnRot;
        hasSavedSpawn = true;

        Debug.Log($"[SceneChangeCollider] Saved spawn pos: {spawnPos} | Collider at: {transform.position}");

        float timeSpent = Time.time - levelStartTime;

        if (directionalLight && SceneManager.GetActiveScene().name != returnSceneName)
        {
            float rotationChange = Mathf.Clamp((timeSpent / 60f) * sunRotationPerMinute, 0f, 90f);
            directionalLight.transform.Rotate(Vector3.right * rotationChange, Space.Self);
        }

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
        fadeCanvas.blocksRaycasts = true;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }

        //yield return StartCoroutine(Fade(1f));
        SceneManager.LoadScene(sceneName);
    }


    private IEnumerator FadeIn()
    {
        fadeCanvas.alpha = 1f;
        fadeCanvas.blocksRaycasts = false;
        
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }
        
        fadeCanvas.alpha = 0f;
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



    /*
    private void MovePlayerToSavedSpawn()
    {
        GameObject player = GameObject.FindGameObjectWithTag(targetTag);
        
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
    }
    */

}
