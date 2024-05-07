using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] int health = 100;
    [SerializeField] bool canAttack = true; // Indicates if the enemy can perform the attack
    [SerializeField] float attackRange = 1.5f;
    [SerializeField] int attackDamage = 10;
    [SerializeField] float attackCooldown = 5f;
    [SerializeField] float despawnTime = 3f;
    [SerializeField] int scoreValue = 10;
    public Transform attackPoint;
    private float lastAttackTime;
    private float currentHealth;
    private bool isDead = false;
    Animator animator;
    Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isAttacking = false;
    private MobSpawner mobSpawner;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = health;
        lastAttackTime = -attackCooldown; // Set last attack time to a value before Time.time
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        mobSpawner = FindObjectOfType<MobSpawner>();
    }

    void Update()
    {
        // Check if canAttack
        if (canAttack && CanAttack() && !isAttacking)
        {
            StartCoroutine(AttackCoroutine());
        }

        bool isMoving = rb.velocity.magnitude > 0.1f;

        // Set animation parameter accordingly
        animator.SetBool("IsMoving", isMoving);
    }

    public void TakeDamage(int damage, Vector2 knockbackDirection, float knockbackForce)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            if(ParameterExists("Hurt")){
                animator.SetTrigger("Hurt");
            }
            // Apply knockback
            rb.velocity = Vector2.zero; // Reset velocity
            rb.AddForce(knockbackDirection.normalized * knockbackForce, ForceMode2D.Impulse);
        }
    }

    void Die()
    {   
        if (isDead) return; // If already dead, do nothing
        isDead = true;
        if(ParameterExists("Hurt")){
            animator.SetTrigger("Hurt");
        }
        if(mobSpawner!=null)
        {
            mobSpawner.EnemyDied();
        }
        //Die Animation
        animator.SetBool("IsDead", true);
        //Disable Movement
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        //Generate Loot
        GetComponent<LootBag>().InstantiateLoot(transform.position);
        //Add Score
        FindObjectOfType<GameManager>().AddScore(scoreValue);
        //Disabling enemy's behavior
        this.enabled = false;
        //Destroy the GameObject after some time
        Destroy(gameObject, despawnTime);
    }

    private bool CanAttack()
    {
        // Check if enough time has passed since the last attack
        if (Time.time - lastAttackTime < attackCooldown)
        {
            return false; // Not ready to attack yet
        }

        // Check if the player is within attack range
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
        foreach (Collider2D playerCollider in hitPlayer)
        {
            if (playerCollider.CompareTag("Player"))
            {
                return true; // Player is within attack range, so the enemy can attack
            }
        }

        return false; // Player is not within attack range
    }

    void Attack()
    {
        // Detect if the player is within attack range
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
        foreach (Collider2D playerCollider in hitPlayer)
        {
            if (playerCollider.CompareTag("Player"))
            {
                // Deal damage to the player
                PlayerCombat playerCombat = playerCollider.GetComponent<PlayerCombat>();
                if (playerCombat != null)
                {
                    playerCombat.TakeDamage(attackDamage);
                }
                // Update attack time
                lastAttackTime = Time.time;
            }
        }
    }

    IEnumerator AttackCoroutine()
    {
        // Set attacking flag to true
        isAttacking = true;

        // Play attack animation
        animator.SetTrigger("Attack");

        // Wait for attack animation to complete
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);

        // Attack logic
        Attack();

        // Wait for attack cooldown
        yield return new WaitForSeconds(attackCooldown);

        // Reset attacking flag
        isAttacking = false;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    bool ParameterExists(string paramName)
    {
        if (animator != null && animator.runtimeAnimatorController != null && animator.parameterCount > 0)
        {
            foreach (AnimatorControllerParameter parameter in animator.parameters)
            {
                if (parameter.name == paramName)
                    return true;
            }
        }
        return false;
    }
}
