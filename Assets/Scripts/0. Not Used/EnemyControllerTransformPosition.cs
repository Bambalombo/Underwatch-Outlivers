using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemyControllerTransformPosition : MonoBehaviour
{
    [Header("Component Connections")]
    [SerializeField] private Vector3Variable playerPosition;
	[SerializeField] private FloatVariable enemyMoveSpeed;
    [SerializeField] private float enemyIndividualMoveSpeed;
    
    void Start()
    {
        SetMoveSpeed();
        StartCoroutine(PerformMovement());
    }

    IEnumerator PerformMovement()
    {
        for (;;)
        {
            while (Vector3.Distance(transform.position,playerPosition.value) > 0.2f)
            {
                transform.position = Vector3.MoveTowards(transform.position,playerPosition.value,enemyIndividualMoveSpeed*Time.deltaTime);
                yield return null;
            }
            yield return new WaitForSeconds(1.5f);
        }
    }
    
    private void SetMoveSpeed()
    {
        enemyIndividualMoveSpeed = enemyMoveSpeed.value * Random.Range(0.85f, 1.15f);
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }
}
