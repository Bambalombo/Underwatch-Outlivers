using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSprite : MonoBehaviour
{
    [SerializeField] private float rotationInterval = 0.25f;
    
    [Range(0, 360)]
    [SerializeField] private float minAngle;
    [Range(0, 360)]
    [SerializeField] private float maxAngle;
    
    private SpriteRenderer _sprite;
    
    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        StartCoroutine(Rotate());
    }

    private IEnumerator Rotate()
    {
        while (true)
        {
            float amount = UnityEngine.Random.Range(minAngle, maxAngle);
            
            ChangeColorOnScale(amount);
            RotateSpriteByAmount(amount);

            if (amount % 2 == 0)
            {
                _sprite.flipX = !_sprite.flipX;
            }
            
            yield return new WaitForSeconds(rotationInterval);
        }
    }

    private void RotateSpriteByAmount(float amount)
    {
        _sprite.transform.Rotate(Vector3.forward, amount);
    }
    
    private void ChangeColorOnScale(float amount)
    {
        float colorScale = (amount - minAngle) / (maxAngle - minAngle) * 152;
        _sprite.color = new Color(colorScale, 255, 255);
    }
}
