using UnityEngine;

public class SpriteFlipper : MonoBehaviour
{
    private Vector3 lastPosition;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        lastPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector3 moveDirection = transform.position - lastPosition;

        // Check if there has been significant horizontal movement
        if (Mathf.Abs(moveDirection.x) > 0.001f)
        {
            // Flip the sprite based on the direction of movement
            if (moveDirection.x > 0)
            {
                spriteRenderer.flipX = false; // Moving right
            }
            else if (moveDirection.x < 0)
            {
                spriteRenderer.flipX = true; // Moving left
            }
        }

        lastPosition = transform.position; // Update the last position
    }
}