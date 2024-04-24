using System.Collections;
using UnityEngine;

public class SpriteShaker : MonoBehaviour
{
    // Public variables to control the shake
    public float shakeAmount = 0.1f; // How much the sprite will move during shaking
    public float shakeDuration = 0.5f; // How long the shaking will last
    public float shakeSpeed = 0.05f; // Delay between each shake step

    // Internal variables
    private Vector3 originalPosition;
    private float shakeEndTime;

    void Awake()
    {
        // Store the original position of the sprite
        originalPosition = transform.localPosition;
        Shake();
    }

    public void Shake()
    {
        shakeEndTime = Time.time + shakeDuration;
        StartCoroutine(ShakeEffect());
    }

    IEnumerator ShakeEffect()
    {
        while (Time.time < shakeEndTime)
        {
            float randomX = Random.Range(-shakeAmount, shakeAmount);
            float randomY = Random.Range(-shakeAmount, shakeAmount);

            transform.localPosition = new Vector3(originalPosition.x + randomX, originalPosition.y + randomY, originalPosition.z);
            yield return new WaitForSeconds(shakeSpeed); // Control the speed of shaking
        }
        // Reset to original position after shaking ends
        transform.localPosition = originalPosition;
    }
}