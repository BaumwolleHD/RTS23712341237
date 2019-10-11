using UnityEngine;
using UnityEditor;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class Damageable : UnitMonoBehaviour
{
    public float currentHp;
    public float maxHp = 1000f;

    private Vector3 initialScale;

    public bool isDead { get { return currentHp < 0f; } }

    void Start()
    {
        currentHp = maxHp;
        initialScale = transform.localScale;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(initialScale * 0.1f, initialScale, currentHp/maxHp);

        if (isDead)
        {
            Destroy();
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