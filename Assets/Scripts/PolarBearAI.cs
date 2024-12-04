//Code Written by Mounir Ziad
using System.Collections;
using UnityEngine;

public class PolarBearAI : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float detectionRange = 5f; // Range within which the polar bear chases the player
    public float moveSpeed = 2f; // Walking speed
    public int damage = 20; // Damage dealt to the player
    public int maxHealth = 3; // Maximum health of the polar bear

    private int currentHealth; // Current health of the polar bear
    private Animator animator;
    private SpriteRenderer spriteRenderer; // For flipping the sprite
    private Vector2 startingPosition; // For idle roaming
    private bool isWalking = false;

    private AudioSource audioSource; // Audio source for the siren sound
    private bool sirenPlaying = false; // Flag to check if the siren is already playing

    void Start()
    {
        currentHealth = maxHealth; // Initialize health
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startingPosition = transform.position;

        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Calculate distance to player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            // Chase the player
            ChasePlayer();
        }
        else
        {
            // Idle roam
            IdleRoam();
        }
    }

    void ChasePlayer()
    {
        isWalking = true;
        animator.SetBool("IsWalking", isWalking);

        // Play siren sound if not already playing
        if (!sirenPlaying)
        {
            audioSource.Play();
            sirenPlaying = true;
        }

        // Move towards the player
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);

        // Flip sprite based on movement direction
        spriteRenderer.flipX = direction.x > 0; // Flip if moving left
    }

    void IdleRoam()
    {
        isWalking = false;
        animator.SetBool("IsWalking", isWalking);

        // Stop siren sound if it's playing
        if (sirenPlaying)
        {
            audioSource.Stop();
            sirenPlaying = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the PlayerController script from the player and apply damage
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(damage);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Reduce health
        StartCoroutine(DamageFlash()); // Trigger damage flash effect
        if (currentHealth <= 0)
        {
            Die(); // Handle death
        }
    }

    private IEnumerator DamageFlash()
    {
        Color originalColor = spriteRenderer.color; // Store the original color
        spriteRenderer.color = Color.red; // Change to red
        yield return new WaitForSeconds(0.2f); // Wait for a short time
        spriteRenderer.color = originalColor; // Revert to the original color
    }

    private void Die()
    {
        // Stop the siren sound
        if (sirenPlaying)
        {
            audioSource.Stop();
        }

        // Play death animation or effects if needed
        Destroy(gameObject); // Destroy the polar bear GameObject
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize detection range in editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
