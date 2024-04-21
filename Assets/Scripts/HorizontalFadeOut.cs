using UnityEngine;

public class HorizontalFadeOut : MonoBehaviour
{
    public float fadeDuration = 0.5f; // Duration of the fade effect
    private SpriteRenderer spriteRenderer;
    private float timeElapsed;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (spriteRenderer != null)
        {
            timeElapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (timeElapsed / fadeDuration));
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);

            if (timeElapsed >= fadeDuration)
            {
                Destroy(gameObject); // Destroy the object after fading out
            }
        }
    }
}