using UnityEngine;
using Xasu.HighLevel;

public class MusicAppLog : MonoBehaviour
{
    void OnEnable()
    {
        AccessibleTracker.Instance.Accessed("MusicApp");
    }

    void OnDisable()
    {

    }
}
