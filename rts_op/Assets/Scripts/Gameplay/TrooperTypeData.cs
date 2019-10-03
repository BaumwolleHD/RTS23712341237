using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrooperTypeData : MonoBehaviour
{
    public static TrooperTypeData instance;
    public TrooperDataType trooper1;
    public TrooperDataType trooper2;
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }
}
public enum TrooperType{trooper1}

[System.Serializable]
public class TrooperDataType
{
    public int maxHp;
    public int xpForLvl2;
    public int xpForLvl3;
    public int primaryAttackDamage;
    public int secondaryAttackDamage;
    public float attackRange;
    public float attackSpeed;
    public float movementSpeed;
}