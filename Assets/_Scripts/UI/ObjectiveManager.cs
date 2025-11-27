using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance;

    [Header("Notes UI Reference")]
    public TMP_Text notesText;
    
    private string currentObjective = "No active objective.";
    private string currentKey;

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
        ("train", "Find the train station."),
        ("supermarket", "Go to the supermarket."),
        ("library", "Head to the library.")
    };

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        AssignRandomObjective();
    }

    public void AssignRandomObjective()
    {
        var obj = possibleObjectives[Random.Range(0, possibleObjectives.Length)];
        currentKey = obj.key;
        currentObjective = obj.text;

        if (notesText != null) notesText.text = currentObjective;

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
