using UnityEngine;
using UnityEditor;

public class Damageable : MonoBehaviour
{
    public float currentHp;
    public float maxHp = 1000f;

    void Start()
    {
        currentHp = maxHp;
    }

    public bool isDead
    {
        get
        {
            return currentHp < 0f;
        }
    }

    public void ApplyDamage(float damage)
    {
        currentHp -= damage;
    }

    public void Heal(float amount)
    {
        currentHp += amount;
    }
}