using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public void startGame()
    {
        //Level01_TrainLevel
        SceneManager.LoadScene("UI_Root", LoadSceneMode.Additive);
        StartCoroutine(DelayedLoadHome());
        /*
        SceneManager.LoadScene("Home");

        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.ResetStats();
        }
        */

    }

    private IEnumerator DelayedLoadHome()
    {
        yield return null;

        GameObject uiRoot = GameObject.Find("UI_Root");
        if (uiRoot != null) uiRoot.SetActive(true);

        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null) canvas.gameObject.SetActive(true);

        SceneManager.LoadScene("Home");

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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
