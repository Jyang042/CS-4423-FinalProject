using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damageAmount = 1; // Damage amount inflicted on the player
    public float despawnDelay = 3f; // Time before the projectile despawns if it doesn't collide with anything

    private bool hasCollided = false; // Flag to track if the projectile has collided

    void Start()
    {
        // Start the despawn timer
        Invoke("DespawnProjectile", despawnDelay);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the projectile has already collided
        if (hasCollided)
        {
            return;
        }

        // Check if the collision is with the player
        if (other.CompareTag("Player"))
        {
            // Damage the player
            other.GetComponent<PlayerCombat>().TakeDamage(damageAmount);
            // Set the flag to true to prevent multiple collisions
            hasCollided = true;
            // Destroy the projectile
            Destroy(gameObject);
        }
        else
        {
            // Destroy the projectile if it collides with anything else
            Destroy(gameObject);
        }
    }

    void DespawnProjectile()
    {
        // Destroy the projectile if it hasn't collided with anything after the despawn delay
        if (!hasCollided)
        {
            Destroy(gameObject);
        }
    }
}
