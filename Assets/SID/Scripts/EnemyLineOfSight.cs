using System;
using UnityEngine;
using Xasu.HighLevel;

public class EnemyLineOfSight : MonoBehaviour
{
    DateTime enteredTime;
    
    private void OnTriggerEnter(Collider other)
    {
        // Make sure <StressMeter> has the name of the component if changes are made
        StressMeter meter = other.GetComponent<StressMeter>();
        if (meter != null)
        {
            meter.stressing = true;
            enteredTime = DateTime.Now;
            GameObjectTracker.Instance.Interacted(transform.parent.name, GameObjectTracker.TrackedGameObject.Enemy).WithResultExtension("http://inside", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StressMeter meter = other.GetComponent<StressMeter>();
        if (meter != null)
        {
            meter.stressing = false;
            GameObjectTracker.Instance.Interacted(transform.parent.name, GameObjectTracker.TrackedGameObject.Enemy)
                .WithResultExtension("http://inside", false)
                .WithDuration(enteredTime, DateTime.Now);
        }
    }
}
