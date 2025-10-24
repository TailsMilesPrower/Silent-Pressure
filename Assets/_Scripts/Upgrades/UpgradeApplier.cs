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
        PlayerStamina stamina = GetComponent<PlayerStamina>();
        if (player == null || PlayerStats.Instance == null)
        {
            return;
        }

        var walkSpeedField = player.GetType().GetField("walkSpeed");
        var runSpeedField = player.GetType().GetField("runSpeed");
        var crouchSpeedField = player.GetType().GetField("crouchSpeed");
        var staminaLimitField = player.GetType().GetField("staminaLimit");

        if (walkSpeedField != null) walkSpeedField.SetValue(player, PlayerStats.Instance.walkSpeed);
        if (runSpeedField != null) runSpeedField.SetValue(player, PlayerStats.Instance.runSpeed);
        if (crouchSpeedField != null) crouchSpeedField.SetValue(player, PlayerStats.Instance.crouchSpeed);
        if (staminaLimitField != null) staminaLimitField.SetValue(player, PlayerStats.Instance.staminaLimit);

        if (stamina != null)
        {
            stamina.maxStamina = PlayerStats.Instance.staminaLimit;
            stamina.currentStamina = Mathf.Min(stamina.currentStamina, stamina.maxStamina);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
