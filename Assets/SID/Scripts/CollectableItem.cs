using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public StoreCollectItems manager;
    public float interactDistance = 3f;

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
            if (manager != null)manager.ItemCollected();
            Destroy(gameObject);
        }
    }
}
