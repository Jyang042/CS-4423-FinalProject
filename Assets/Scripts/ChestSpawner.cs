using UnityEngine;
using UnityEngine.Tilemaps;

public class ChestSpawner : MonoBehaviour
{
    public GameObject chestPrefab;
    public int totalNumberOfChests = 10; // Total number of chests to spawn
    public float chestHeightAbovePlatform = 1.0f; // Height above the platform to spawn chests

    void Start()
    {
        SpawnChests();
    }

    void SpawnChests()
    {
        Tilemap[] platforms = FindObjectsOfType<Tilemap>(); // Find all Tilemap objects in the scene

        int chestsSpawned = 0;

        // Find the tilemap named "platform"
        Tilemap platformTilemap = null;
        foreach (Tilemap platform in platforms)
        {
            if (platform.gameObject.layer == LayerMask.NameToLayer("Platforms"))
            {
                platformTilemap = platform;
                break;
            }
        }

        if (platformTilemap == null)
        {
            Debug.LogWarning("No Tilemap named 'platform' found.");
            return;
        }

        // Calculate the total number of cells in the "platform" tilemap
        BoundsInt platformBounds = platformTilemap.cellBounds;
        int totalCells = platformBounds.size.x * platformBounds.size.y;

        // Loop until desired number of chests is spawned or no more valid cells
        while (chestsSpawned < totalNumberOfChests && totalCells > 0)
        {
            // Select a random cell within the platform bounds
            Vector3Int randomCell = new Vector3Int(Random.Range(platformBounds.min.x, platformBounds.max.x), Random.Range(platformBounds.min.y, platformBounds.max.y), 0);

            // Check if there's a tile at the selected cell
            if (!platformTilemap.HasTile(randomCell))
            {
                totalCells--;
                continue; // Skip this cell if it doesn't have a tile
            }

            // Check if there's empty space above the selected cell
            Vector3Int checkPosition = randomCell + Vector3Int.up;
            if (platformTilemap.HasTile(checkPosition))
            {
                continue; // Skip this cell if there's a tile above it
            }

            // Spawn a chest at this position
            Vector3 spawnPosition = platformTilemap.CellToWorld(randomCell) + new Vector3(0, chestHeightAbovePlatform, 0);
            Instantiate(chestPrefab, spawnPosition, Quaternion.identity);
            chestsSpawned++;

            // Decrement the total number of cells
            totalCells--;
        }
    }
}
