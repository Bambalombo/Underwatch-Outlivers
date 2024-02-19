using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private FloatVariable attackSpeed;
    [SerializeField] private GameObject bullet;

    private Coroutine _shootingSequence;
    private void Start()
    {
        RestartBasicAttack();
    }

    IEnumerator BasicAttack(float attacksPerSecond)
    {
        for (;;)
        {
            Instantiate(bullet);
            yield return new WaitForSeconds(1/attacksPerSecond);
        }
    }

    public void RestartBasicAttack()
    {
        if (_shootingSequence != null) 
            StopCoroutine(_shootingSequence);
        _shootingSequence = StartCoroutine(BasicAttack(attackSpeed.value));
    }
}
