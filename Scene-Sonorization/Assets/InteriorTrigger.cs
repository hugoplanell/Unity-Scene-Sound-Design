using UnityEngine;
using UnityEngine.Audio;

public class InteriorTrigger : MonoBehaviour
{
    public AudioMixerSnapshot interiorSnapshot; // Reference to the interior snapshot
    public AudioMixerSnapshot exteriorSnapshot; // Reference to the exterior snapshot

    // This method is called when a trigger collider attached to a child object detects a collision
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is relevant (e.g., the player)
        if (other.CompareTag("Player"))
        {
            // Set the mixer snapshot to the interior snapshot
            // Set the player footstep sound to the interior sound
            
            interiorSnapshot.TransitionTo(0.5f); // Transition to the interior snapshot over 0.5 seconds
            Debug.Log("The player entered the trigger!");

            other.GetComponent<Footsteps>().interiorFootstepSounds = true; // Set the interior footstep sounds
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the collided object is relevant (e.g., the player)
        if (other.CompareTag("Player"))
        {
            // Set the mixer snapshot to the exterior snapshot
            // Set the player footstep sound to the exterior sound
            
            exteriorSnapshot.TransitionTo(0.5f); // Transition to the exterior snapshot over 0.5 seconds
            Debug.Log("The player exited the trigger!");

            other.GetComponent<Footsteps>().interiorFootstepSounds = false; // Reset the interior footstep sounds
        }
    }
}