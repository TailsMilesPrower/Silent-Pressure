using UnityEngine;
using Xasu.HighLevel;

public class EmailAppLog : MonoBehaviour
{
    void OnEnable()
    {
        AccessibleTracker.Instance.Accessed("EmailApp");
    }

    void OnDisable()
    {

    }
}
