using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Building))]
public class Basis : NetMonoBehaviour
{
    public BaseData playerData;

    void Awake()
    {
    }

    void Start()
    {
        transform.position = ((Vector3[])PhotonNetwork.CurrentRoom.CustomProperties["basePosition"])[OwnerActorNumber-1];
        ((PlayerManager)photonView.Owner.TagObject).basisBuilding = this;
    }

    private void Update()
    {
    }
}
