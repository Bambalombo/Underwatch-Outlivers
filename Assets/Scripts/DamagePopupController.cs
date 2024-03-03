using UnityEngine;
using TMPro;

public class DamagePopupController : MonoBehaviour
{
    [SerializeField] private TextMeshPro damageText;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float fadeSpeed = 1f;
    

    private void Update()
    {
        DamagePopupAnimation();
    }

    private void DamagePopupAnimation()
    {
        // Move the popup up and fade out over time
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        //transform.position += Vector3.up * (moveSpeed * Time.deltaTime);
        var color = damageText.color;
        color.a -= fadeSpeed * Time.deltaTime;
        damageText.color = color;

        // Destroy the popup when it's fully transparent
        if (color.a <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetDamage(float damage)
    {
        damageText.text = damage.ToString();
    }
}