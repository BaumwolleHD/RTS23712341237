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
    public UnitData unitData;
    public PlayerManager unitOwner;
    public int currentXp;
    public Damageable attackTarget;
    public float timeBeforeAttack = 0f;

    public bool attackOnCooldown { get { return timeBeforeAttack > 0f; } }
    public bool isNPC { get { return unitOwner == null; } }

    public void Start()
    {
        damageable.currentHp = unitData.maxHp;

        if (pathfinder.speed != unitData.movementSpeed) pathfinder.speed = unitData.movementSpeed;
    }

    void Update()
    {
        HandleAttacking();
    }

    public void SetMaterial(Material newMat)
    {
        MeshRenderer meshRenderer = transform.Find("Offset/Torso").GetComponentInChildren<MeshRenderer>();
        if (newMat != meshRenderer.sharedMaterial)
        {
            meshRenderer.sharedMaterial = newMat;
        }
    }

    #region Movement
    
    public void WalkToBase()
    {
        WalkTo(unitOwner.basisBuilding.transform.position);
    }

    public void WalkTo(Vector3 destination)
    {
        pathfinder.destination = destination;
    }
    #endregion

    #region Attacking

    public void RequestAttack(Unit target)
    {
        RequestAttack(target.damageable);
    }

    /// <summary>
    /// Löst einen Angriff aus, wenn der Angriff nicht auf Cooldown ist, unabhängig davon, ob dies Sinn macht
    /// </summary>
    /// <param name="attackTarget">Ziel des Angriffes</param>
    public void RequestAttack(Damageable attackTarget)
    {
        if (!attackOnCooldown)
        {
            PerformAttack(attackTarget);
        }
    }

    void PerformAttack(Damageable attackTarget)
    {
        attackTarget.ApplyDamage(unitData.primaryAttackDamage);
        timeBeforeAttack = 1 / unitData.attackSpeed;
    }

    void HandleAttacking()
    {
        timeBeforeAttack -= Time.deltaTime;

        if (attackTarget)
        {
            Debug.Log(Vector3.Distance(transform.position, attackTarget.transform.position));
            if (Vector3.Distance(transform.position, attackTarget.transform.position) > unitData.attackRange)
            {
                WalkTo(attackTarget.transform.position);
            }
            else
            {
                pathfinder.isStopped = true;
                RequestAttack(attackTarget);
                if (attackTarget.GetComponent<Unit>()) attackTarget.GetComponent<Unit>().RequestAttack(this);
            }
        }
        else
        {

            pathfinder.isStopped = false;
        }
    }

    public void Attack(Damageable newUnitToAttack)
    {
        attackTarget = newUnitToAttack;
    }

    public bool CanAttack(Damageable target)
    {
        Unit targetUnit = target.GetComponent<Unit>();
        return !targetUnit || targetUnit.unitOwner != unitOwner;
    }
    #endregion
}