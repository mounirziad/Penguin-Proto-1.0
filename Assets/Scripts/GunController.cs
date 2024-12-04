//Code written by Jason Westcott

using UnityEngine;
using UnityEngine.InputSystem;

public class GunController : MonoBehaviour
{
    public Transform player; // Reference to the player
    public Camera mainCamera; // Reference to the main camera
    public GameObject bulletPrefab; // Reference to the bullet prefab
    public Transform EndOfGun; // Reference to the position of the gun's barrel (EndOfGun)
    public float bulletSpeed = 10f; // Speed of the bullet
    public float shootCooldown = 0.2f; // Cooldown time between shots
    private float lastShotTime; // Time of the last shot

    private AudioSource gunAudioSource; // Reference to the AudioSource

    void Start()
    {
        // Get the AudioSource component attached to the gun
        gunAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Get mouse position in world space
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the direction from the player to the mouse
        Vector2 direction = mousePosition - player.position;

        // Calculate the angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Get the player's facing direction (check scale.x)
        bool isFacingLeft = player.localScale.x < 0;

        // Flip the angle if the player is facing left
        if (isFacingLeft)
        {
            angle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        }

        // Rotate the gun
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Shooting logic
        if (Input.GetMouseButton(0) && Time.time > lastShotTime + shootCooldown) // Left-click to shoot
        {
            ShootBullet();
            lastShotTime = Time.time; // Update last shot time
        }
    }

    // Function to shoot a bullet
    void ShootBullet()
    {
        // Create a bullet at the EndOfGun's position with the same rotation as the gun
        GameObject bullet = Instantiate(bulletPrefab, EndOfGun.position, transform.rotation);

        // Get the bullet's Rigidbody2D component
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        // Calculate the direction the gun is facing. If the gun is facing left, use -transform.right.
        Vector2 shootDirection = transform.right;  // shoot to the right by default

        // If the gun is facing left, reverse the direction
        if (player.localScale.x < 0) // If player is facing left
        {
            shootDirection = -transform.right; // Bullet goes left
        }

        // Apply velocity to the bullet in the direction the gun is pointing (purely horizontal)
        bulletRb.velocity = shootDirection * bulletSpeed;

        // Play gunshot sound
        if (gunAudioSource != null)
        {
            gunAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("No AudioSource found on the gun!");
        }
    }
}
