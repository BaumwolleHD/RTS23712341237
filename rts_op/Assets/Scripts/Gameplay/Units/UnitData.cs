using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "UnitData")]
public class UnitData : ScriptableObject
{
    public string displayName = "No name";
    public float maxHp = 1000;
    public int xpForLvl2 = 20;
    public int xpForLvl3 = 50;
    public int primaryAttackDamage = 100;
    public int secondaryAttackDamage = 20;
    public float attackRange = 2f;
    public float attackSpeed = 0.5f;
    public float movementSpeed = 30;
}