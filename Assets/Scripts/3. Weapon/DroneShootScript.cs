using System.Collections;
using UnityEngine;

public class DroneShootScript : MonoBehaviour
{
    //[SerializeField] private FloatVariable playerAttackSpeed;
    [SerializeField] private FloatVariable basicBulletSpeed;
   // [SerializeField] private Vector3Variable cursorPosition;

    [SerializeField] private float damage = 10f;

    private float _bulletSpeed;
    private Vector3 _bulletDirection;
    private PlayerStatsController _playerStatsController;
    public GameObject _drone;
    

    private void Start()
    {
        //TODO: Does not work with more players
        _playerStatsController = GameObject.FindWithTag("Player").GetComponent<PlayerStatsController>();
        
        transform.position = _drone.transform.position;
        //_bulletSpeed = basicBulletSpeed.value + _playerStatsController.GetAttackSpeed()/2; //TODO: Bullet speed should not be attack speed
        //_bulletDirection = (cursorPosition.value).normalized;

      //  StartCoroutine(SendBulletFlying());
        
    }

    private IEnumerator SendBulletFlying()
    {
        for (;;)
        {
            transform.Translate(_bulletDirection * (_bulletSpeed * Time.deltaTime));
            yield return null;

          //  GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
          //  Destroy(projectile, 5f); // Destroys the projectile after 5 seconds
        }
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<EnemyCombatController>();
            if (enemy != null)
            {
                enemy.EnemyTakeDamage(damage);
                
                // Destroy the bullet
                Destroy(gameObject);
            }
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}