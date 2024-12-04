//Code written by Jason Westcott and Mounir Ziad

using System.Collections;
using UnityEngine;

public class PresentConsumable : MonoBehaviour
{
    public float speedBoost = 3f; // Amount to increase speed
    public float boostDuration = 10f; // Duration for speed boost

    private void OnTriggerStay2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller != null)
        {
            // Start the speed boost if the player hasn't already received it
            if (!controller.IsSpeedBoosted)
            {
                StartCoroutine(SpeedBoostCoroutine(controller));
                Destroy(gameObject); // Destroy the present after collision
            }
        }
    }

    private IEnumerator SpeedBoostCoroutine(PlayerController controller)
    {
    float originalSpeed = controller.speed; // Store original speed
    controller.speed += speedBoost; // Increase the speed
    controller.IsSpeedBoosted = true;

    yield return new WaitForSeconds(boostDuration); // Wait for the duration of the boost

    controller.speed = originalSpeed; // Reset speed to original value
    controller.IsSpeedBoosted = false;
    }
}
