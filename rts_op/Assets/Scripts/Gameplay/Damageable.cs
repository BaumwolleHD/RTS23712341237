using UnityEngine;
using UnityEditor;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]

public class Damageable : UnitMonoBehaviour
{
    public float currentHp;
    public float maxHp = 1000f;
    private Unit lastDamageSource;
    
    public Vector3 initialScale;

    public bool isDead { get { return currentHp < 0f; } }

    void Start()
    {
        currentHp = maxHp;
        initialScale = transform.localScale;
        Debug.Log(initialScale);
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(initialScale * 0.1f, initialScale, currentHp/maxHp);

        if (isDead)
        {
            lastDamageSource.Killed(this);
            Destroy();
            if (GetComponent<NeutralUnit>())
            {
                GetComponentInParent<Camp>().CampNeutralUnitDies();
            }
        }
    }

    public void ApplyDamage(Unit source, float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Min(currentHp, maxHp);
        if(source) lastDamageSource = source;
    }

    public void Heal(float amount)
    {
        currentHp += amount;
    }
    
}