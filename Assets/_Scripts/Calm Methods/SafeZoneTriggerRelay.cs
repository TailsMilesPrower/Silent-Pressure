using UnityEngine;

public class SafeZoneTriggerRelay : MonoBehaviour
{
    public BreathingMechanic breathingMechanic;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<SafeZone>())
        {
            Debug.Log("Player entered safe zone: " + other.name);
            if (breathingMechanic) breathingMechanic.SetSafeZone(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<SafeZone>())
        {
            Debug.Log("Player exited safe zone: " + other.name);
            if (breathingMechanic) breathingMechanic.SetSafeZone(false);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
