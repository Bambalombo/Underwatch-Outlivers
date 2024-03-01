using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BasicBulletController : MonoBehaviour
{
    [SerializeField] private FloatVariable playerAttackSpeed;
    [SerializeField] private FloatVariable basicBulletSpeed;
    [SerializeField] private Vector3Variable cursorPosition, playerPosition;

    [SerializeField] private float damage = 10f; // Add this line

    private float _bulletSpeed;
    private Vector3 _bulletDirection;
    
    void Start()
    {
        transform.position = playerPosition.value;
        _bulletSpeed = basicBulletSpeed.value + playerAttackSpeed.value/2;
        _bulletDirection = (cursorPosition.value).normalized;
        // _bulletDirection.z = (Mathf.Atan2(_bulletDirection.y, _bulletDirection.x) * Mathf.Rad2Deg);
        // transform.Rotate(0,0,(Mathf.Atan2(_bulletDirection.y, _bulletDirection.x) * Mathf.Rad2Deg)/2);
        // transform.localRotation = Quaternion.Euler(0,0,(Mathf.Atan2(_bulletDirection.y, _bulletDirection.x) * Mathf.Rad2Deg)/2);
        
        StartCoroutine(SendBulletFlying());
        StartCoroutine(KillTimer());
    }

    private IEnumerator SendBulletFlying()
    {
        for (;;)
        {
            transform.Translate(_bulletDirection * (_bulletSpeed * Time.deltaTime));
            yield return null;
        }
    }

    private IEnumerator KillTimer()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject); // Destroy the bullet after it hits the enemy
            }
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
