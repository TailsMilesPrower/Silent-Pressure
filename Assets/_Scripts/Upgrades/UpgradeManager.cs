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

    private List<string> availableUpgrades = new List<string>()
    {
        "Increase Walk Speed",
        "Increase Run Speed"
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
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.ApplyUpgrade(upgradeType);
        }

        SceneManager.LoadScene("Level02_TestLevel");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
