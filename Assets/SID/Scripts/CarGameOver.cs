using UnityEngine;
using UnityEngine.SceneManagement;
using Xasu.HighLevel;

public class CarGameOver: MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CompletableTracker.Instance.Initialized("Death of Car", CompletableTracker.CompletableType.Session);
            SceneManager.LoadScene("GameOver");
        }
    }
}
