using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public Terrain terrain; // Reference to the Terrain component

    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip[] grassFootsteps; // Array of grass footstep sounds
    public AudioClip[] dirtFootsteps; // Array of dirt footstep sounds
    public AudioClip[] concreteFootsteps; // Array of concrete footstep sounds
    public float stepInterval = 0.5f; // Time interval between steps
    public float pitchVariation = 0.2f; // Pitch variation range

    public bool interiorFootstepSounds = false; // Flag for interior footstep sounds

    private float stepTimer = 0f;

    // Map texture indices to terrain types
    private readonly int[] grassTextures = { 0, 1 }; // Indices for grass textures
    private readonly int[] dirtTextures = { 2, 3, 4 };  // Indices for dirt textures
    private readonly int[] concreteTextures = {}; // Indices for concrete textures

    void Update()
    {
        if (IsWalking())
        {
            if (IsRunning())
            {
                stepInterval = 0.2f;
            }
            else
            {
                stepInterval = 0.3f;
            }

            stepTimer += Time.deltaTime;

            if (stepTimer >= stepInterval)
            {
                PlayFootstepSound();
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    private bool IsWalking()
    {
        // Replace with your own logic to detect if the player is walking
        return Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0;
    }

    private bool IsRunning()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }

    private void PlayFootstepSound()
    {
        AudioClip footstepClip = GetFootstepClip();

        if (footstepClip != null)
        {
            // Randomize pitch for variety
            audioSource.pitch = 1f + Random.Range(-pitchVariation, pitchVariation);
            audioSource.PlayOneShot(footstepClip);
        }
    }

    private AudioClip GetFootstepClip()
    {
        if (interiorFootstepSounds)
        {
            return GetRandomClip(concreteFootsteps);
        }

        string terrainType = GetTerrainType();

        switch (terrainType)
        {
            case "Grass":
                return GetRandomClip(grassFootsteps);
            case "Dirt":
                return GetRandomClip(dirtFootsteps);
            case "Concrete":
                return GetRandomClip(concreteFootsteps);
            default:
                return null; // No sound for unknown terrain
        }
    }

    private AudioClip GetRandomClip(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return null;
        return clips[Random.Range(0, clips.Length)];
    }

    private string GetTerrainType()
    {
        if (terrain == null) return "Unknown";

        Vector3 playerPosition = transform.position;
        TerrainData terrainData = terrain.terrainData;

        Vector3 terrainPosition = terrain.transform.position;
        int mapX = Mathf.FloorToInt((playerPosition.x - terrainPosition.x) / terrainData.size.x * terrainData.alphamapWidth);
        int mapZ = Mathf.FloorToInt((playerPosition.z - terrainPosition.z) / terrainData.size.z * terrainData.alphamapHeight);

        float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

        float grassValue = GetTextureGroupValue(splatmapData, grassTextures);
        float dirtValue = GetTextureGroupValue(splatmapData, dirtTextures);
        float concreteValue = GetTextureGroupValue(splatmapData, concreteTextures);

        if (grassValue > dirtValue && grassValue > concreteValue) return "Grass";
        if (dirtValue > grassValue && dirtValue > concreteValue) return "Dirt";
        if (concreteValue > grassValue && concreteValue > dirtValue) return "Concrete";

        return "Unknown";
    }

    private float GetTextureGroupValue(float[,,] splatmapData, int[] textureIndices)
    {
        float value = 0f;
        foreach (int index in textureIndices)
        {
            value += splatmapData[0, 0, index];
        }
        return value;
    }
}