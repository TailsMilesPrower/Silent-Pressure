using UnityEngine;

public class CarSpawn: MonoBehaviour
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
            Instantiate(prefab, spawnPoint.position, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(2f, 5f));
        }
    }
}
