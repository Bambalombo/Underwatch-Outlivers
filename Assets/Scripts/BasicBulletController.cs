using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BasicBulletController : MonoBehaviour
{
    [SerializeField] private FloatVariable playerAttackSpeed;
    [SerializeField] private FloatVariable basicBulletSpeed;
    [SerializeField] private Vector3Variable cursorPosition, playerPosition;
    
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

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
