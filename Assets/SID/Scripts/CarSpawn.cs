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
            RandomizeCarMaterial(car);
            yield return new WaitForSeconds(Random.Range(2f, 5f));
        }
    }

    private void RandomizeCarMaterial(GameObject car)
    {
        Renderer rend = car.GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            Material mat = rend.material;

            mat.color = new Color(Random.value, Random.value,Random.value);

            // An attempt was made but failed. Nothing changes but nothing breaks so I leave it here.
            if (mat.HasProperty("_Metallic"))
                mat.SetFloat("_Metallic", Random.Range(0f, 1f));

            if (mat.HasProperty("_Glossiness"))
                mat.SetFloat("_Glossiness", Random.Range(0f, 1f));
        }
    }
}
