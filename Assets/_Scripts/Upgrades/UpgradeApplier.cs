using UnityEngine;

public class UpgradeApplier : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ApplyStatsToPlayer();
    }

    void ApplyStatsToPlayer()
    {
        PlayerController player = GetComponent<PlayerController>();
        if (player == null || PlayerStats.Instance == null)
        {
            return;
        }

        var walkSpeedField = player.GetType().GetField("walkSpeed");
        var runSpeedField = player.GetType().GetField("runSpeed");

        if (walkSpeedField != null) walkSpeedField.SetValue(player, PlayerStats.Instance.walkSpeed);
        if (runSpeedField != null) walkSpeedField.SetValue(player, PlayerStats.Instance.walkSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
