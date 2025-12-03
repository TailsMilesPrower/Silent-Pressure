using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public Transform player;

    private static Vector3 townSpawnPos;
    private static bool townSpawnSaved = false;

    private static Transform returnEntrance;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SaveTownSpawn(Transform spawn)
    {
        townSpawnPos = spawn.position;
        townSpawnSaved = true;
    }

    public void RegisterEntrance(Transform t)
    {
        returnEntrance = t;
    }

    public void LoadTown()
    {
        SceneManager.LoadScene("Town");

        SceneManager.sceneLoaded += (scene, mode) =>
        {
            if (player == null) return;

            if (!townSpawnSaved)
            {
                player.position = GameObject.Find("DefaultTownSpawn").transform.position;
                return;
            }

            if (returnEntrance != null)
            {
                player.position = returnEntrance.position;
                return;
            }

            player.position = townSpawnPos;

        };
    }

    /*
    public void LoadHomeOrTherapy()
    {
        string next = "Home";

        ObjectiveManager om = FindObjectOfType<ObjectiveManager>();
        if (om != null && om.HasTherapistKey)
        {
            next = "Therapy Office"
        }
    }
    */
}
