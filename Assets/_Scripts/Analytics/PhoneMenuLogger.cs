using UnityEngine;
using Xasu.HighLevel;

public class PhoneMenuLogger : MonoBehaviour
{
    void OnEnable()
    {
        AccessibleTracker.Instance.Accessed("Phone");
    }

    void OnDisable()
    {

    }
}
