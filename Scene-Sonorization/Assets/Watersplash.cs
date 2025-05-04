using UnityEngine;

public class Watersplash : MonoBehaviour
{
    public Terrain terrain; // Reference to the Terrain component
    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip waterSplashSound; // Sound for water splash
    public float waterHeightThreshold = 1.0f; // Height below which the player is considered in water

    public float stepInterval = 0.5f; // Time interval between steps
    public float pitchVariation = 0.2f; // Pitch variation range

    private float stepTimer = 0f; // Timer to track intervals between splashes
    private Vector3 lastPosition; // To track movement

    void Update()
    {
        if (IsInWater() && IsMoving())
        {
            stepTimer += Time.deltaTime;

            if (stepTimer >= stepInterval)
            {
                PlayWaterSplashSound();
                stepTimer = 0f; // Reset the timer after playing the sound
            }
        }
        else
        {
            stepTimer = 0f; // Reset the timer if the player is not in water or not moving
        }

        // Update the last position for the next frame
        lastPosition = transform.position;
    }

    private bool IsInWater()
    {
        if (terrain == null) return false;

        Vector3 playerPosition = transform.position;
        float terrainHeight = terrain.SampleHeight(playerPosition);

        Debug.Log(terrainHeight < waterHeightThreshold ? "In water" : "Not in water");

        return terrainHeight < waterHeightThreshold;
    }

    private bool IsMoving()
    {
        // Check if the player has moved since the last frame
        return Vector3.Distance(transform.position, lastPosition) > 0.01f; // Adjust threshold as needed
    }

    private void PlayWaterSplashSound()
    {
        Debug.Log("Playing water splash sound");

        if (waterSplashSound != null)
        {
            // Randomize pitch for variety
            audioSource.pitch = 1f + Random.Range(-pitchVariation, pitchVariation);
            audioSource.PlayOneShot(waterSplashSound);
        }
    }
}