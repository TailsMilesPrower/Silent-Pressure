using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeCollider : MonoBehaviour
{

    [SerializeField] private string SceneName;
    [SerializeField] private string targetTag = "Player";
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            SceneManager.LoadScene("Upgrades");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
