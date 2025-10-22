using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void startGame()
    {
        SceneManager.LoadScene("Level01_TrainLevel");
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.ResetStats();
        }

    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void backToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
