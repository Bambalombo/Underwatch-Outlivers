using UnityEngine;

public class FadeInAndScaleSpriteEffect : MonoBehaviour
{
    public float fadeInDuration = 2.0f;  // Duration in seconds for the fade to complete
    public float targetScale = 1.5f;     // The scale the GameObject should reach
    public Vector3 initialScale = Vector3.one * 0.5f; // Starting scale of the GameObject

    private SpriteRenderer spriteRenderer; // To hold the sprite renderer
    private float timeElapsed;             // To track the time elapsed since the start of the effect

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on the GameObject.");
            return;
        }

        // Set the initial opacity and scale
        Color c = spriteRenderer.color;
        c.a = 0; // Set alpha to 0 to make the sprite invisible initially
        spriteRenderer.color = c;

        transform.localScale = initialScale; // Set initial scale
    }

    void Update()
    {
        if (timeElapsed < fadeInDuration)
        {
            // Calculate the fraction of the duration that has passed
            float fracComplete = timeElapsed / fadeInDuration;

            // Update the sprite's alpha
            Color c = spriteRenderer.color;
            c.a = fracComplete; // Increase alpha based on time
            spriteRenderer.color = c;

            // Update the scale
            transform.localScale = Vector3.Lerp(initialScale, Vector3.one * targetScale, fracComplete);

            // Increment the time elapsed
            timeElapsed += Time.deltaTime;
        }
    }
}