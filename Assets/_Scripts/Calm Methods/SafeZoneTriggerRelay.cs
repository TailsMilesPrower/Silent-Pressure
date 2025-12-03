using UnityEngine;

public class SafeZoneTriggerRelay : MonoBehaviour
{
    public BreathingMechanic breathingMechanic;

    private void OnTriggerEnter(Collider other)
    {
        var zone = other.GetComponent<SafeZone>();
        if (zone)
        {
            Debug.Log("Player entered safe zone: " + other.name);
            breathingMechanic.SetSafeZone(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var zone = other.GetComponent<SafeZone>();
        if (zone)
        {
            Debug.Log("Player exited safe zone: " + other.name);
            breathingMechanic.SetSafeZone(false);
        }
    }
}
