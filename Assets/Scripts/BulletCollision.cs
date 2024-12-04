//Code written by Jason Westcott and Mounir Ziad
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    public GameObject particleEffectPrefab; // Reference to the particle effect prefab
    public int bulletDamage = 1; // Damage dealt by the bullet

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the bullet collided with the polar bear
        if (collision.gameObject.CompareTag("PolarBear"))
        {
            // Get the PolarBearAI script and apply damage
            PolarBearAI polarBearAI = collision.gameObject.GetComponent<PolarBearAI>();
            if (polarBearAI != null)
            {
                polarBearAI.TakeDamage(bulletDamage);
            }
        }

        // Instantiate the particle effect at the collision position
        Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);

        // Destroy the bullet after the collision
        Destroy(gameObject);
    }
}
