using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Xasu;
using Xasu.HighLevel;

public class GameProgressLogger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        XasuTracker.Instance.DefaultActor = new TinCan.Agent{ name = System.Guid.NewGuid().ToString() };
        await Task.Yield();
        CompletableTracker.Instance.Initialized("Silent Pressure", CompletableTracker.CompletableType.Game);
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
    }

    private void SceneManager_activeSceneChanged(Scene current, Scene next)
    {
        AccessibleTracker.Instance.Accessed(next.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        CompletableTracker.Instance.Completed("Silent Pressure", CompletableTracker.CompletableType.Game, Time.realtimeSinceStartup);
    }
}
