using UnityEngine;
using Xasu.HighLevel;

public class SettingsAppLog : MonoBehaviour
{
    void OnEnable()
    {
        AccessibleTracker.Instance.Accessed("SettingsApp");
    }

    void OnDisable()
    {

    }
}
