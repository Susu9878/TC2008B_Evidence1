using UnityEngine;

public class BoxSpawn : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject box;

    [Header("Spawn Area")]
    public float minX = 0f;
    public float maxX = 27f;
    public float minY = 0.7f;
    public float maxY = 2f;
    public float minZ = 0f;
    public float maxZ = 42f;

    [Header("Shelves Setup")]
    public int numberOfShelves = 5;   
    public int boxesPerShelf = 5;     

    void Start()
    {
        int totalBoxes = numberOfShelves * boxesPerShelf;

        for (int i = 0; i < totalBoxes; i++)
            SpawnRandomPrefab();
    }

    void Update()
    {
 
        if (Input.GetKeyDown(KeyCode.Space))
            SpawnRandomPrefab();
    }

    public void SpawnRandomPrefab()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        float randomZ = Random.Range(minZ, maxZ);

        Vector3 randomPosition = new Vector3(randomX, randomY, randomZ);

        GameObject newBox = Instantiate(box, randomPosition, Quaternion.identity);

      
        newBox.tag = "box";
        newBox.layer = LayerMask.NameToLayer("PickupBox");
    }
}