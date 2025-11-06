using UnityEngine;

public class SafeZone : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if (TryGetComponent(out Collider c)) Gizmos.DrawCube(transform.position, c.bounds.size);
        //Gizmos.DrawCube(transform.position, GetComponent<Collider>().bounds.size);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
