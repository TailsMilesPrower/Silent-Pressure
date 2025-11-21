using UnityEngine;
using Xasu;
using Xasu.HighLevel;

public class TutorialLogger : MonoBehaviour
{
    void OnEnable()
    {
        AccessibleTracker.Instance.Accessed("Tutorial");
    }

    void OnDisable()
    {
        
    }
}
