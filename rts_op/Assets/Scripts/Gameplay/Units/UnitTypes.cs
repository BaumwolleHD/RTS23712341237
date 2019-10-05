using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTypes : MonoBehaviour
{
    public UnitData trooper1;
    public UnitData trooper2;

    public static UnitTypes instance;
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }
}

[System.Serializable]
public class UnitData
{
    public string name;
    public float maxHp;
    public int xpForLvl2;
    public int xpForLvl3;
    public int primaryAttackDamage;
    public int secondaryAttackDamage;
    public float attackRange;
    public float attackSpeed;
    public float movementSpeed;
}