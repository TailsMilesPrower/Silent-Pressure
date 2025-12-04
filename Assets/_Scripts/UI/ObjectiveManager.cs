using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance;

    [Header("Notes UI Reference")]
    public TMP_Text notesText;
    
    private string currentObjective = "No active objective.";
    private string currentKey;

    public bool HasNewObjective { get; set; } = false;

    /* // Old info
    [Header("Current Objective")]
    [TextArea(2, 5)]

    [Header("Possible Objectives (Randomized)")]
    [TextArea(2, 5)]
    public string[] objectivePool = new string[]
    {
        "Find the train station",
        "Go to the supermarket.",
        "Head to the library"
    };
    */

    /* // Old Awake
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
    }
    */

    private (string key, string text)[] possibleObjectives =
    {
        ("train", "Catch the train at the Subway station."),
        ("supermarket", "Go to the supermarket."),  
        //("library", "Head to the library.")
    };

    private static bool initialized = false;
    private int progressionIndex = 0;

    private void Awake()
    {
        //SceneManager.sceneLoaded += OnSceneLoaded;
        //UpdateUI();

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
    }

    /*
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RebindUI();
    }

    private void RebindUI()
    {
        notesText = GameObject.Find("Note_Text")?.GetComponent<TMP_Text>();
    }
    */

    private void Start()
    {
        if (!initialized)
        {
            initialized = true;
            AssignRandomObjective();
        }
        
        UpdateUI();

        /*
        else
        {
            if (notesText != null) notesText.text = currentObjective;
        }
        */
    }

    public void UpdateUI()
    {
        if (notesText != null) notesText.text = currentObjective;
    }

    public void AssignRandomObjective()
    {
        var obj = possibleObjectives[Random.Range(0, possibleObjectives.Length)];
        currentKey = obj.key;
        currentObjective = obj.text;
        HasNewObjective = true;
        UpdateUI();

        //if (notesText != null) notesText.text = currentObjective;
        //int index = Random.Range(0, possibleObjectives.Length);
        //currentObjective = possibleObjectives[index];
    }

    public string GetObjective()
    {
        return currentObjective;
    }

    public string GetObjectiveKey()
    {
        return currentKey;
    }

    public void AdvanceStoryObjective() //This is the feature after finishing a level
    {
        progressionIndex++;

        if (progressionIndex == 1)
        {
            currentKey = "therapist";
            currentObjective = "Visit your therapist.";
        }
        else if (progressionIndex == 2)
        {
            currentKey = "home";
            currentObjective = "Return home.";
        }
        else
        {
            //AssignRandomObjective();
        }

        HasNewObjective = true;
        UpdateUI();

        /* //Old version
        switch (progressionIndex)
        {
            case 1:
                currentKey = "therapist";
                currentObjective = "Visit your therapist.";
                break;

            case 2:
                currentKey = "home";
                currentObjective = "Return home.";
                break;

            default:
                AssignRandomObjective();
                break;
        }

        if (notesText != null) notesText.text = currentObjective;
        */
    }

    /* // Old methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RandomizeObjective();
    }

    public void RandomizeObjective()
    {
        if (objectivePool == null || objectivePool.Length == 0)
        {
            currentObjective = "No objectives available.";
        }
        else
        {
            int index = Random.Range(0, objectivePool.Length);
            currentObjective = objectivePool[index];
        }

        UpdateNotesUI();
    }

    public void SetObjective(string newObjective)
    {
        currentObjective = newObjective;
        UpdateNotesUI();
    }

    public void UpdateNotesUI()
    {
        if (notesText != null) notesText.text = currentObjective;
    }
    */
}
