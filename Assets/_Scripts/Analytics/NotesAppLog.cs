using UnityEngine;
using Xasu.HighLevel;

public class NotesAppLog : MonoBehaviour
{
    void OnEnable()
    {
        AccessibleTracker.Instance.Accessed("NotesApp");
    }

    void OnDisable()
    {

    }
}
