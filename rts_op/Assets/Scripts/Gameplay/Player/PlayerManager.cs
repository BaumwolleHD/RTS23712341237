using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : NetMonoBehaviour
{
    public Basis basisBuilding;
    public int playerNumber;
    public UnitSelection unitSelection;

    private void Awake()
    {
        unitSelection = GetComponent<UnitSelection>();

        if (isRealPlayer && PhotonNetwork.IsConnected)
        {
            photonView.Owner.TagObject = this;
        }
        playerNumber = gameManager.allPlayers.Count;
        gameManager.allPlayers.Add(this);
    }

    public bool isRealPlayer
    {
        get
        {
            return GetComponent<AIPlayerManager>()==null;
        }
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            if(isRealPlayer) //TODO: Put in subclass
            {
                GetComponent<Hotkeys>().enabled = true;
                GetComponent<Camera>().enabled = true;
                GetComponent<AudioListener>().enabled = true;
                GetComponent<UnitSelection>().enabled = true;
                Cursor.lockState = CursorLockMode.Confined;
            }

            Basis newBase = InstanciateOnGround("base_lvl1_with_textures", 0, 0).GetComponent<Basis>();
            basisBuilding = newBase;
            basisBuilding.owningPlayer = this;
        }

        gameManager.playerCount++;
    }

    public void SpawnUnit(Unit unitToSpawn)
    {
        InstanciateOnGround(unitToSpawn.name, basisBuilding.transform.position.x+8f, basisBuilding.transform.position.z + 8f).GetComponent<Unit>().unitOwner = this;
    }
}
