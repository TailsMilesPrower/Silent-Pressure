using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        SceneManager.LoadScene("UI_Root", LoadSceneMode.Additive);
        SceneManager.LoadScene("Home", LoadSceneMode.Single);
    }
}
