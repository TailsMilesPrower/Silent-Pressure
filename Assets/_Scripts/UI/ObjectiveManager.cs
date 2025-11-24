using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance;

    [Header("Current Objective")]
    [TextArea(2, 5)]
    public string currentObjective = "No active objective.";

    [Header("Possible Objectives (Randomized)")]
    [TextArea(2, 5)]
    public string[] objectivePool = new string[]
    {
        "Find the train station",
        "Go to the supermarket.",
        "Head to the library"
    };

    [Header("Notes UI Reference")]
    public TMP_Text notesText;

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
}
