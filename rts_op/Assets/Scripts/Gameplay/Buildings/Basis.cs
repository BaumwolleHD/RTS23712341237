using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Building))]
public class Basis : NetMonoBehaviour
{
    public BaseData playerData;
    public PlayerManager owningPlayer;

    void Awake()
    {
    }

    void Start()
    {
        transform.position = ((Vector3[])PhotonNetwork.CurrentRoom.CustomProperties["basePosition"])[owningPlayer.playerNumber];
        PutOnGround();
        if(owningPlayer.isRealPlayer)
        {
            Camera.main.GetComponent<CameraMovement>().LookAtBase();
        }
    }

    private void Update()
    {

    }
}
