using System.Collections;
using UnityEngine;

public class PlayerDefaultShoot : MonoBehaviour
{
    //[SerializeField] private FloatVariable attackSpeed;
    [SerializeField] private GameObject bullet;
    private Transform _bulletParent;
    private PlayerStatsController _playerStatsController;
    private Coroutine _shootingSequence;

    private void Awake()
    {
        _bulletParent = GameManager.GetBulletParent();
        
        _playerStatsController = GetComponent<PlayerStatsController>();
    }

    private void Start()
    {
        RestartBasicAttack();
    }

    private IEnumerator BasicAttack(float attacksPerSecond)
    {
        for (;;)
        {
            Instantiate(bullet, _playerStatsController.GetPlayerPosition(), Quaternion.identity, _bulletParent);
            
            yield return new WaitForSeconds(1 / attacksPerSecond);
        }
    }

    private void RestartBasicAttack()
    {
        if (_shootingSequence != null) 
            StopCoroutine(_shootingSequence);
        _shootingSequence = StartCoroutine(BasicAttack(_playerStatsController.GetAttackSpeed()));
    }
}
