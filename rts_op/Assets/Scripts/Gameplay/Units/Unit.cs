using Photon.Pun;
using System;
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
    public UnitType unitData;
    public CurrentUnitData currentUnitData;
    public PlayerManager unitOwner;
    public Damageable attackTarget;
    public float timeBeforeAttack = 0f;

    public TextMesh debugText;

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
        HandleLevelUp();
    }

    private void HandleLevelUp()
    {
        while (unitData.xpForLvlUp.Length >= currentUnitData.level && currentUnitData.xp >= unitData.xpForLvlUp[currentUnitData.level - 1])
        {
            currentUnitData.level++;
            damageable.initialScale *= 1.5f;
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
        attackTarget.ApplyDamage(this, unitData.primaryAttackDamage);
        timeBeforeAttack = 1 / unitData.attackSpeed;
    }

    void HandleAttacking()
    {
        timeBeforeAttack -= Time.deltaTime;

        if (attackTarget)
        {
            if (Vector3.Distance(transform.position, attackTarget.transform.position) > unitData.attackRange)
            {
                pathfinder.isStopped = false;
                WalkTo(attackTarget.transform.position);
            }
            else
            {
                pathfinder.isStopped = true;
                pathfinder.velocity = Vector3.zero;
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

    public void Killed(Damageable killedDamageble)
    {
        if (killedDamageble.GetComponent<Unit>())
        {
            currentUnitData.xp += killedDamageble.GetComponent<Unit>().currentUnitData.level * 4;
        }
        else if (killedDamageble.GetComponent<Camp>())
        {
            killedDamageble.GetComponent<Camp>().DropLoot(this);
        }
        
    }

    public bool CanAttack(Damageable target)
    {
        Unit targetUnit = target.GetComponent<Unit>();
        if (targetUnit)
        {
            return targetUnit.unitOwner != unitOwner;
        }
        return true;
    }
    #endregion
}