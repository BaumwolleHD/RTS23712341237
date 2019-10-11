using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
camps:
    -   building, das neutralunits spawnt
	-	spawnrate + ort --> MapMagic
	-	Loot: gegner droppen: xp(für killer-unit + spieler-xp)
			->	chance auf: gold, mehr xp, Aquarius/Mana, einfache Forschung etc.
    -   CampTypes
	-	Respawn-Cooldown
	-	Spawn-Time
	-	irgendeine möglichkeit den Loot von der Häufigkeit des Clearens oder anderen Faktoren abhängig zu machen
*/

[RequireComponent(typeof(Building))]

public class Camp : NetMonoBehaviour
{
    public NeutralUnit unitType;
    public float timeToRespawn;
    public CampType campType;

    void Start()
    {
        timeToRespawn = campType.firstSpawnTime;
        GetComponent<Damageable>().enabled = false;
    }

    private void Update()
    {
        HandleSpawning();
    }

    private void HandleSpawning()
    {
        if (timeToRespawn < gameManager.gameTime && transform.childCount == 0)
        {
            for (int i = 0; i < Random.Range(campType.minUnitCount, campType.maxUnitCount); i++)
            {
                GameObject newUnit = PhotonNetwork.InstantiateSceneObject(unitType.name, transform.position, new Quaternion());
                newUnit.transform.parent = transform;
            }
            GetComponent<Damageable>().enabled = false;
        }
    }

    int CalculateLootValue(int minDrop, int maxDrop)
    {
        return Mathf.FloorToInt( Mathf.Pow(1 - campType.dropDecayRate, gameManager.gameTime) * Random.Range(minDrop, maxDrop));
    }

    public void DropLoot(Unit killerUnit)
    {
        killerUnit.currentUnitData.xp += CalculateLootValue(campType.minXpDrop, campType.maxXpDrop);
        switch (Random.Range(0,3))
        {
            case 0:
                killerUnit.ownerPlayerManager.basisBuilding.playerData.gold += CalculateLootValue(campType.maxGoldDrop, campType.maxGoldDrop);
                break;
            case 1:
                killerUnit.ownerPlayerManager.basisBuilding.playerData.xp += CalculateLootValue(campType.maxXpDrop, campType.maxXpDrop);
                break;
            case 2:
                killerUnit.ownerPlayerManager.basisBuilding.playerData.mana += CalculateLootValue(campType.maxManaDrop, campType.maxManaDrop);
                break;
        }
    }

    public void CampNeutralUnitDies()
    {
        int numberOfDeadNeutralUnitsInChildren = 0;
        foreach (NeutralUnit neutralUnit in GetComponentsInChildren<NeutralUnit>())
        {
            if (neutralUnit.damageable.isDead)
            {
                numberOfDeadNeutralUnitsInChildren++;
            }
        }
        if (numberOfDeadNeutralUnitsInChildren == GetComponentsInChildren<NeutralUnit>().Length)
        {
            timeToRespawn = gameManager.gameTime + campType.respawnCooldown;
            GetComponent<Damageable>().enabled = true;
        }
    }
}
