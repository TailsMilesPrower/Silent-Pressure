using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Background Music")]
    public AudioSource backgroundSource;
    public AudioClip backgroundTrack;
    public float volume = 0.7f;

    private ListenToMusic musicSystem;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (backgroundSource == null)
        {
            backgroundSource = gameObject.AddComponent<AudioSource>();
            backgroundSource.loop = true;
            backgroundSource.playOnAwake = false; 
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (backgroundTrack != null)
        {
            backgroundSource.clip = backgroundTrack;
            backgroundSource.volume = volume;
            backgroundSource.Play();
        }

        FindMusicSystem();
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        FindMusicSystem();
    }

    private void FindMusicSystem()
    {
        ListenToMusic found = FindObjectOfType<ListenToMusic>(true);
        if (found != null)
        {
            musicSystem = found;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (musicSystem == null) return;

        HandleMusicState();
    }

    private void HandleMusicState()
    {
        if (musicSystem.IsSongPlaying())
        {
            if (backgroundSource.isPlaying)
            {
                backgroundSource.Stop();
            }
        }
        else
        {
            if (!backgroundSource.isPlaying)
            {
                backgroundSource.clip = backgroundTrack;
                backgroundSource.volume = volume;
                backgroundSource.Play();
            }
        }
    }
}
