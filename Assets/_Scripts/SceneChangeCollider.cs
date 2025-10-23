using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeCollider : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Name of the scene to load. Leave empty to use build index.")]
    [SerializeField] private string sceneName;

    //public int sceneBuildIndex = -1;

    [Header("Trigger Settings")]
    [SerializeField] private string targetTag = "Player";

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(targetTag)) return;


        if (!string.IsNullOrEmpty(sceneName))
        {
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


    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
