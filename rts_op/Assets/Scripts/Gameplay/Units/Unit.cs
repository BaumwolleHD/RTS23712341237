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
public class Unit : NetMonoBehaviour
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
        GetComponent<Damageable>().currentHp = unitData.maxHp;

        if (GetComponent<NavMeshAgent>().speed != unitData.movementSpeed)
        {
            GetComponent<NavMeshAgent>().speed = unitData.movementSpeed;
        }

        MeshRenderer meshRenderer = transform.Find("Offset/Torso").GetComponentInChildren<MeshRenderer>();
        if (hasOwner)
        {
            Material newMat = unitOwner.playerNumber == 1 ? gameManager.mapSettings.teamMaterials[0] : gameManager.mapSettings.teamMaterials[1];
            if (newMat != meshRenderer.sharedMaterial)
            {
                meshRenderer.sharedMaterial = newMat;
            }
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

    public void RequestAttack(Unit unit)
    {
        RequestAttack(unit.GetComponent<Damageable>());
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
        if (GetComponent<Damageable>().isDead)
        {
            Destroy();
        }
    }

    public void Attack(Damageable newUnitToAttack)
    {
        attackTarget = newUnitToAttack;
    }

    public bool CanAttack(Damageable damageable)
    {
        Unit targetUnit = damageable.GetComponent<Unit>();
        return !targetUnit  || targetUnit.unitOwner != unitOwner;
    }
}

/*
#if UNITY_EDITOR
[CustomEditor(typeof(Unit))]
public class TrooperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Unit thisTrooper = target as Unit;
        

        if (!Application.isPlaying && !GameObject.Find("GlobalData"))
        {
            EditorSceneManager.OpenScene("Assets/Scenes/Menu.unity", OpenSceneMode.Additive);
        }

        Uni trooperTypeData = GameObject.Find("GlobalData").GetComponent<UnitTypes>();

        FieldInfo[] fields = typeof(UnitTypes).GetFields();
        List<string> trooperTypes = new List<string>();
        int count = 0;

        int currentIndex = 0;
        foreach (FieldInfo field in fields)
        {
            count++;
            if (field.Name != "instance")
            {
                string name = ((UnitData)field.GetValue(trooperTypeData)).name;
                trooperTypes.Add("(" + count.ToString() + ") " + (name != "" ? name : "Namenlos"));
                if (((UnitData)field.GetValue(trooperTypeData)) == thisTrooper.trooperDataType)
                {
                    currentIndex = count - 1;
                }

            }
        }
        if(thisTrooper.lastChoiceIndex == -1)
        {
            currentIndex = 0;
        }
        int choiceIndex = EditorGUILayout.Popup("Unit-type", currentIndex, trooperTypes.ToArray());
        if(choiceIndex != thisTrooper.lastChoiceIndex)
        {
            thisTrooper.trooperDataType = (UnitData)trooperTypeData.GetType().GetFields()[choiceIndex].GetValue(trooperTypeData);
            thisTrooper.lastChoiceIndex = choiceIndex;
        }

        if (!Application.isPlaying)
        {
            thisTrooper.Start();
        }

        thisTrooper.GetComponent<Damageable>().maxHp = thisTrooper.trooperDataType.maxHp;
    }
}
#endif

    */