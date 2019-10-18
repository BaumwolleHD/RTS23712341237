using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemies : UnitMonoBehaviour
{
    public float aggroRange = 15f;
    
    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, aggroRange);

        float nearestDistance = 9999f;
        Damageable nearstTarget = null;

        foreach(Collider collider in hitColliders)
        {
            float thisDist = Vector3.Distance(transform.position, collider.transform.position);
            Damageable target = collider.GetComponentInParent<Damageable>();

            if (target && target.enabled && target != damageable && unit.WantToAttack(target) && thisDist < nearestDistance)
            {
                nearestDistance = thisDist;
                nearstTarget = target;
            }
        }

        if(nearstTarget)
        {
            unit.Attack(nearstTarget);
        }
    }
}
