//Code written by Jason Westcott and Mounir Ziad
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //Set up variables for health bar
    public float speed; // Speed of main slider transition
    public float speedEffect; // Speed of effect slider transition
    
    //Set up slider variables
    public Slider Fill; // Main slider to display current health
    public Slider FillEffect; // Slider for a smooth effect of health changes

    private PlayerController player; // Reference to the PlayerController

    // Start is called before the first frame update
    void Start()
    {
        // Find the player in the scene or set it via the inspector
        player = FindObjectOfType<PlayerController>();

        if (player != null)
        {
            // Initially, set the fill values to the max health of the player
            Fill.maxValue = player.maxHealth;
            FillEffect.maxValue = player.maxHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        // Set the target values to a lerp and multiply it 
        // by speed while maintaining the max health to get a smooth transition.
        float target = Mathf.Lerp(Fill.value, player.currentHealth, speed * Time.deltaTime); 
        float targetEffect = Mathf.Lerp(FillEffect.value, player.currentHealth, speedEffect * Time.deltaTime);

        // Set fill value and effect value to new target and target effect
        Fill.value = target; 
        FillEffect.value = targetEffect;
    }
}
