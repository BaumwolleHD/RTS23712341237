using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "UnitType")]

public class UnitType : ScriptableObject
{
    public string displayName = "No name";
    public float maxHp = 1000;
    public int primaryAttackDamage = 100;
    public int secondaryAttackDamage = 20;
    public float attackRange = 2f;
    public float attackSpeed = 0.5f;
    public float movementSpeed = 30;
    public int[] xpForLvlUp = new int[] {20, 50};
}

[System.Serializable]
public struct CurrentUnitData
{
    public int xp;
    public int hp;
    public int level;
    public float attackRange;
    public float attackSpeed;
    public float movementSpeed;
    public int primaryAttackDamage;
    public int secondaryAttackDamage;
}