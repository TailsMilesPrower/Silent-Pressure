using UnityEngine;

public class SafeZone : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if (TryGetComponent(out Collider c)) Gizmos.DrawCube(transform.position, c.bounds.size);
        //Gizmos.DrawCube(transform.position, GetComponent<Collider>().bounds.size);
    }
}
