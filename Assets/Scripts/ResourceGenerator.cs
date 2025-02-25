using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    [Header("Spawn Area Settings")]
    public Vector2 zoneSize = new Vector2(10, 10); // Width and height of the spawning area
    public Vector2 cellSize = new Vector2(2, 2);  // Width and height of each cell
    [Range(0, 1)] public float spawnProbability = 0.2f;
    
    public GameObject[] resourcePrefabs;         // Pool of resource prefabs to spawn

    private Vector2 bottomLeftCorner;            // Bottom-left corner of the spawn zone

    private void Start()
    {
        InitializeSpawnArea();
        GenerateResources();
    }

    private void InitializeSpawnArea()
    {
        // Calculate the bottom-left corner of the spawn zone relative to the generator's position
        bottomLeftCorner = (Vector2)transform.position - (zoneSize / 2);
    }

    private void GenerateResources()
    {
        // Calculate the number of cells horizontally and vertically
        int columns = Mathf.FloorToInt(zoneSize.x / cellSize.x);
        int rows = Mathf.FloorToInt(zoneSize.y / cellSize.y);

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                // Calculate the position of the cell's center
                Vector2 cellPosition = bottomLeftCorner + new Vector2(x * cellSize.x + cellSize.x / 2, y * cellSize.y + cellSize.y / 2);

                // Randomly decide whether to spawn a resource in this cell
                if (Random.value > spawnProbability) // chance to spawn a resource
                {
                    SpawnResource(cellPosition);
                }
            }
        }
    }

    private void SpawnResource(Vector2 position)
    {
        // Randomly select a resource prefab from the pool
        GameObject resourcePrefab = resourcePrefabs[Random.Range(0, resourcePrefabs.Length)];
        
        // Instantiate the resource at the calculated position
        Instantiate(resourcePrefab, position, Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the spawning zone in the editor
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, zoneSize);

        // Draw individual cells
        int columns = Mathf.FloorToInt(zoneSize.x / cellSize.x);
        int rows = Mathf.FloorToInt(zoneSize.y / cellSize.y);

        Vector2 bottomLeft = (Vector2)transform.position - (zoneSize / 2);

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                Vector2 cellCenter = bottomLeft + new Vector2(x * cellSize.x + cellSize.x / 2, y * cellSize.y + cellSize.y / 2);
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(cellCenter, cellSize);
            }
        }
    }
}
