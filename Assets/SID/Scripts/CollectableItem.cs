using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public StoreCollectItems manager;
    public float interactDistance = 3f; // how close the player must be

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);

        if (dist <= interactDistance && Input.GetKeyDown(KeyCode.F))
        {
            // Notify the manager
            if (manager != null)
                manager.ItemCollected();

            // Destroy this collectible
            Destroy(gameObject);
        }
    }
}
