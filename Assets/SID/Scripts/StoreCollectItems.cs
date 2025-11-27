using UnityEngine;

public class StoreCollectItems: MonoBehaviour
{
    [Header("Objects to collect")]
    public CollectibleItem[] collectibles; // assign your 6 collectibles here

    [Header("Object to enable when all collected")]
    public GameObject objectToEnable;


    private int collectedCount = 0;

    private void Start()
    {
        if (objectToEnable != null)
            objectToEnable.SetActive(false);
    }

    public void ItemCollected()
    {
        collectedCount++;

        if (collectedCount >= collectibles.Length)
        {
            // All collected!
            objectToEnable.SetActive(true);
        }
    }
}
