using System.Collections;
using UnityEngine;

public class HealingBulletController : MonoBehaviour
{
    private float _speed;
    private int _healAmount;
    private Transform _target;

    [SerializeField] private float destroyTime = 10f;

    public void Initialize(Transform target, float speed, WeaponStats weaponStats)
    {
        _target = target; // Set the target as the player who fired the bullet
        _speed = speed;
        //_healAmount = weaponStats.GetHealingAmount();  // Ensure this method exists and returns an integer

        StartCoroutine(SendHealingBulletFlying());
        Destroy(gameObject, destroyTime);
    }

    private IEnumerator SendHealingBulletFlying()
    {
        while (true)
        {
            // Continuously calculate the direction towards the target player
            Vector2 direction = (_target.position - transform.position).normalized;
            transform.Translate(direction * (_speed * Time.deltaTime), Space.World);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            // Trigger healing if the bullet hits the player who fired it
            other.GetComponentInParent<PlayerHealthController>().PlayerHeal(1f);
            Destroy(gameObject);
        }
    }
}