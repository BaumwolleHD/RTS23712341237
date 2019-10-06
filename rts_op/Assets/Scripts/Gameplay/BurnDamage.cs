using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class BurnDamage : MonoBehaviour
{
    public float dps = 10f;
    
    void FixedUpdate()
    {
        GetComponent<Damageable>().ApplyDamage(Time.deltaTime * dps);
    }
}
