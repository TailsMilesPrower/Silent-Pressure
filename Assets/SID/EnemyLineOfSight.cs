using UnityEngine;

public class EnemyLineOfSight : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Make sure <StressMeter> has the name of the component if changes are made
        StressMeter meter = other.GetComponent<StressMeter>();
        if (meter != null)
        {
            meter.stressing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StressMeter meter = other.GetComponent<StressMeter>();
        if (meter != null)
        {
            meter.stressing = false;
        }
    }
}
