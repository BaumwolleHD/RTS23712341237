using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(TransformSynchronizer))]
[RequireComponent(typeof(Damageable))]
public class Unit : UnitMonoBehaviour
{
    public int currentXp;

    [HideInInspector]
    public int lastChoiceIndex = -1;

    public UnitData unitData;

    public PlayerManager unitOwner;

    public Damageable attackTarget;

    public float timeBeforeAttack = 0f;

    public bool attackOnCooldown { get { return timeBeforeAttack > 0f; } }

    public void Start()
    {
        damageable.currentHp = unitData.maxHp;

        if (GetComponent<NavMeshAgent>().speed != unitData.movementSpeed)
        {
            GetComponent<NavMeshAgent>().speed = unitData.movementSpeed;
        }

        if (!isNPC)
        {
            WalkToBase();
        }
        else
        {
            WalkTo(transform.position + (Vector3.one * Random.Range(0.2f,1f)));
        }
    }

    public void SetMaterial(Material newMat)
    {
        MeshRenderer meshRenderer = transform.Find("Offset/Torso").GetComponentInChildren<MeshRenderer>();
        if (newMat != meshRenderer.sharedMaterial)
        {
            meshRenderer.sharedMaterial = newMat;
        }
    }

    public bool isNPC { get { return unitOwner == null; } }

    public void WalkToBase()
    {
        WalkTo(unitOwner.basisBuilding.transform.position);
    }

    public void WalkTo(Vector3 destination)
    {
        GetComponent<NavMeshAgent>().destination = destination;
    }

    void Update()
    {
        HandleAttacking();
        HandleDeath();
    }

    public void RequestAttack(Unit target)
    {
        RequestAttack(target.damageable);
    }

    public void RequestAttack(Damageable attackTarget)
    {
        if(!attackOnCooldown)
        {
            PerformAttack(attackTarget);
        }
    }

    void PerformAttack(Damageable attackTarget)
    {
        attackTarget.ApplyDamage(unitData.primaryAttackDamage);
        timeBeforeAttack = 1/unitData.attackSpeed;
    }

    void HandleAttacking()
    {
        timeBeforeAttack -= Time.deltaTime;

        if(attackTarget)
        {
            Debug.Log(Vector3.Distance(transform.position, attackTarget.transform.position));
            if(Vector3.Distance(transform.position, attackTarget.transform.position) > unitData.attackRange)
            {
                WalkTo(attackTarget.transform.position);
            }
            else
            {
                GetComponent<NavMeshAgent>().isStopped = true;
                RequestAttack(attackTarget);
                if (attackTarget.GetComponent<Unit>()) attackTarget.GetComponent<Unit>().RequestAttack(this);
            }
        }
        else
        {

            GetComponent<NavMeshAgent>().isStopped = false;
        }
    }

    void HandleDeath()
    {
        if (damageable.isDead)
        {
            Destroy();
        }
    }

    public void Attack(Damageable newUnitToAttack)
    {
        attackTarget = newUnitToAttack;
    }

    public bool CanAttack(Damageable target)
    {
        Unit targetUnit = target.GetComponent<Unit>();
        return !targetUnit  || targetUnit.unitOwner != unitOwner;
    }
}