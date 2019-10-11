using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "CampType")]

public class CampType : ScriptableObject
{
    public float firstSpawnTime = 1;
    public int minUnitCount = 1;
    public int maxUnitCount = 3;
    public int minGoldDrop = 50;
    public int maxGoldDrop = 500;
    public int minManaDrop = 6;
    public int maxManaDrop = 12;
    public int minXpDrop = 50;
    public int maxXpDrop = 75;
    public float respawnCooldown = 4.63f;
    public float dropDecayRate = 0.001f;
}