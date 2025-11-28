using UnityEngine;

public class CarSpawn : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject prefab;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private System.Collections.IEnumerator SpawnLoop()
    {
        while (true)
        {
            GameObject car = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

            // Randomize the material on this specific car
            RandomizeCarMaterial(car);

            yield return new WaitForSeconds(Random.Range(2f, 5f));
        }
    }

    private void RandomizeCarMaterial(GameObject car)
    {
        // Get renderer (assuming the car has a MeshRenderer)
        Renderer rend = car.GetComponentInChildren<Renderer>();

        if (rend != null)
        {
            // This creates a *unique* runtime material instance
            Material mat = rend.material;

            // Randomize the color
            mat.color = new Color(
                Random.value,        // R
                Random.value,        // G
                Random.value         // B
            );

            // Optional: random metallic and smoothness (if using Standard shader)
            if (mat.HasProperty("_Metallic"))
                mat.SetFloat("_Metallic", Random.Range(0f, 1f));

            if (mat.HasProperty("_Glossiness"))
                mat.SetFloat("_Glossiness", Random.Range(0f, 1f));
        }
    }
}
