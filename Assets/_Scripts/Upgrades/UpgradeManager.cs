using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    [Header("UI References")]
    public Button upgradeButton1;
    public Button upgradeButton2;
    public TextMeshProUGUI upgradeText1;
    public TextMeshProUGUI upgradeText2;

    [Header("Level Settings")]
    public int totalLevels = 10;
    public string levelPrefix = "Level";
    public string endSceneName = "ComingSoon";

    private List<string> availableUpgrades = new List<string>()
    {
        "Increase Walk Speed",
        "Increase Run Speed",
        "Increase Crouch Speed",
        "Increase Stamina Limit"
    };

    private string selectedUpgrade1;
    private string selectedUpgrade2;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateRandomUpgrades();

        upgradeButton1.onClick.AddListener(() => ApplyUpgrade(selectedUpgrade1));
        upgradeButton2.onClick.AddListener(() => ApplyUpgrade(selectedUpgrade2));
    }

    public void GenerateRandomUpgrades()
    {
        selectedUpgrade1 = GetRandomUpgrade();
        selectedUpgrade2 = GetRandomUpgrade();

        upgradeText1.text = selectedUpgrade1;
        upgradeText2.text = selectedUpgrade2;
    }

    string GetRandomUpgrade()
    {
        int randomIndex = Random.Range(0, availableUpgrades.Count);
        return availableUpgrades[randomIndex];
    } 

    public void ApplyUpgrade(string upgradeType)
    {
        if (PlayerStats.Instance == null)
        {
            Debug.LogWarning("No PlayerStats found in scene");
        }

        PlayerStats.Instance.ApplyUpgrade(upgradeType);
        PlayerStats.Instance.upgradesVisited++;
        LoadNextLevel();

        //SceneManager.LoadScene("Level02_TestLevel");
    }

    void LoadNextLevel()
    {
        int visitCount = PlayerStats.Instance.upgradesVisited;
        
        if (visitCount <= totalLevels - 1)
        {
            string nextLevelIndex = (visitCount + 1).ToString("00");
            string nextSceneName = FindSceneByPrefix(levelPrefix + nextLevelIndex + "_");
            
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                Debug.Log("Loading next scene: " + nextSceneName);
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogWarning("No scene for prefix found: " + levelPrefix + nextLevelIndex + "_");
                SceneManager.LoadScene(endSceneName);
            }

        }
        else
        {
            SceneManager.LoadScene(endSceneName);
        }
    }

    string FindSceneByPrefix(string prefix)
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneCount;  i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(path);
            if (sceneName.StartsWith(prefix))
            {
                return sceneName;
            }
        }

        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
