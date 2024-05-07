using System.Collections;
using UnityEngine;

public class ProjectileThrower : MonoBehaviour
{
    public GameObject projectilePrefab; // Reference to the projectile prefab
    public float projectileSpeed = 5f; // Speed of the projectile
    public float spawnOffset = 1f; // Offset distance to spawn the projectile in front of the enemy

    private Transform player; // Reference to the player's transform
    private Transform enemyTransform; // Reference to the enemy's transform

    void Start()
    {
        // Find the player GameObject by tag
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // Get the enemy's transform
        enemyTransform = transform;

        // Start the coroutine to fire projectiles
        StartCoroutine(FireProjectiles());
    }

    IEnumerator FireProjectiles()
    {
        while (true) // Keep firing projectiles indefinitely
        {
            // Check if the enemy has line of sight to the player
            if (HasLineOfSight())
            {
                // Calculate the direction towards the player
                Vector2 direction = (player.position - enemyTransform.position).normalized;

                // Calculate the position to spawn the projectile
                Vector3 spawnPosition = enemyTransform.position + (Vector3)direction * spawnOffset;

                // Instantiate the projectile prefab at the spawn position
                GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

                // Get the projectile's Rigidbody2D component and set its velocity
                Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                rb.velocity = direction * projectileSpeed;

                // Debug statement to log when a projectile is spawned
                Debug.Log("Projectile spawned");

                // Wait for some time before firing the next projectile
                yield return new WaitForSeconds(1f);
            }
            else
            {
                // Debug statement to log that line of sight is blocked
                Debug.Log("Line of sight blocked");
            }

            // Wait for a short duration before checking line of sight again
            yield return new WaitForSeconds(0.1f);
        }
    }

    bool HasLineOfSight()
    {
        // Perform a raycast from the enemy towards the player
        RaycastHit2D hit = Physics2D.Raycast(enemyTransform.position, player.position - enemyTransform.position);

        // Draw a line to visualize the line of sight (for debugging)
        Debug.DrawLine(enemyTransform.position, player.position, Color.red);

        // If the raycast hits the player, return true
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            return true;
        }

        // If the raycast hits an obstacle or doesn't hit anything, return false
        return false;
    }
}
