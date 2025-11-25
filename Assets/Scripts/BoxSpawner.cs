using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    public GameObject box; 
    public float minX = 0f;
    public float maxX = 27f;
    public float minY = .7f; 
    public float maxY = 2f;
    public float minZ = 0f;
    public float maxZ = 42f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnRandomPrefab();
        }
    }

    public void SpawnRandomPrefab()
    {
        // Generate random coordinates
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        float randomZ = Random.Range(minZ, maxZ);

        Vector3 randomPosition = new Vector3(randomX, randomY, randomZ);
        Instantiate(box, randomPosition, Quaternion.identity);
    }

}
