using UnityEngine;

public class PlayNotification : MonoBehaviour
{
    private bool triggered = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if(other.CompareTag("Player"))
        {
            triggered = true;
            Debug.Log("Trigger entered, sending notification...");
            ObjectiveManager.Instance.AssignRandomObjective();
            NotificationManager.Instance.TriggerNotification();
        }
    }
}
