using System.Collections;
using UnityEngine;

public class PlayerDefaultShoot : MonoBehaviour
{
    [SerializeField] private FloatVariable attackSpeed;
    [SerializeField] private GameObject bullet;
    private Transform _bulletParent;
    
    private Coroutine _shootingSequence;

    private void Awake()
    {
        // TODO: Can be optimized but i don't have the energy to fix it right now
        _bulletParent = GameObject.FindWithTag("BulletsParent").transform;
    }

    private void Start()
    {
        RestartBasicAttack();
    }

    private IEnumerator BasicAttack(float attacksPerSecond)
    {
        for (;;)
        {
            Instantiate(bullet, _bulletParent, false);
            
            yield return new WaitForSeconds(1 / attacksPerSecond);
        }
    }

    private void RestartBasicAttack()
    {
        if (_shootingSequence != null) 
            StopCoroutine(_shootingSequence);
        _shootingSequence = StartCoroutine(BasicAttack(attackSpeed.value));
    }
}
