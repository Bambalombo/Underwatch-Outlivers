using UnityEngine;
using UnityEngine.Serialization;

public class BossBullet : MonoBehaviour
{
    [SerializeField] private float damage; //TODO: I can's remove SerializeField here for some reason
    
    public void SetDamage(float value)
    {
        damage = value;
    }
    public float GetDamage() => damage;
}
