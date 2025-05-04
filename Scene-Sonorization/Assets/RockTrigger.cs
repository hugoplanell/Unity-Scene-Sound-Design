using UnityEngine;

public class RockTrigger : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip triggerSound; // Sound to play when the player enters the trigger

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Play the sound
            if (audioSource != null && triggerSound != null)
            {
                audioSource.PlayOneShot(triggerSound);
            }
        }
    }
}