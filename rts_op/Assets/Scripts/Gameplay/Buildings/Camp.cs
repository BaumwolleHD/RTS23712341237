using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camp : NetMonoBehaviour
{
    public int minUnitCount;
    public int maxUnitCount;
    public NeutralUnit unitType;
    public float firstSpawnTime;
    bool hasSpawned;

    private void Update()
    {
        if (firstSpawnTime < gameManager.gameTime && !hasSpawned)
        {
            for (int i = 0; i < Random.Range(minUnitCount, maxUnitCount); i++)
            {
                GameObject newUnit = PhotonNetwork.InstantiateSceneObject(unitType.name, transform.position, new Quaternion());
                newUnit.transform.parent = transform;

            }
            hasSpawned = true;
        }
    }


}
