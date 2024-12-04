//Code written by Jason Westcott and Mounir Ziad
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;


public class PlayerController : MonoBehaviour, Player
{
    [SerializeField] private DialogueUI dialogueUI;

    public DialogueUI DialogueUI => dialogueUI;

    public IInteractable Interactable { get; set; }

    public int health { get { return currentHealth; } }

    public InputAction MoveAction;
    public Animator anim;

    private Rigidbody2D rigidbody2d;
    private SpriteRenderer spriteRenderer; // Declare the SpriteRenderer here

    private Vector2 move;

    public int maxHealth = 100;

    [HideInInspector] public int currentHealth;

    public float speed = 3.0f;

    public bool IsSpeedBoosted { get; set; } // Flag to check if speed boost is active

    public float timeInvincible = 2.0f;
    private bool isInvincible;
    private float damageCooldown;

    private AudioSource audioSource; // Reference to the AudioSource for walking sound

    void Start()
    {
        currentHealth = maxHealth;
        MoveAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Initialize the SpriteRenderer

        // Get the AudioSource component for the walking sound
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (DialogueUI.IsOpen) return;

        move = MoveAction.ReadValue<Vector2>();

        if (Mathf.Abs(move.x) > 0.01f || Mathf.Abs(move.y) > 0.01f)
        {
            anim.SetBool("IsRunning", true);

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            anim.SetBool("IsRunning", false);

            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;

        if (direction.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime;
            if (damageCooldown < 0)
            {
                isInvincible = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Interactable != null)
            {
                Interactable.Interact(this);
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 position = (Vector2)rigidbody2d.position + move * speed * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
            {
                return;
            }
            isInvincible = true;
            damageCooldown = timeInvincible;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        StartCoroutine(DamageFlash()); // Trigger the damage flash
        ChangeHealth(-damage);
    }

    public void Heal(int amount)
    {
        ChangeHealth(amount);
    }

    private IEnumerator DamageFlash()
    {
        Color originalColor = spriteRenderer.color; // Store the original color
        spriteRenderer.color = Color.red; // Change to red
        yield return new WaitForSeconds(0.2f); // Wait for a short time
        spriteRenderer.color = originalColor; // Revert to the original color
    }
}
