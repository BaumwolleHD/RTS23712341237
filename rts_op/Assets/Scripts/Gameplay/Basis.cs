using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basis : MonoBehaviourPun
{
    void Start()
    {
        string basePosition = "base" + photonView.Owner.ActorNumber.ToString() + "Position";
        transform.position = (Vector3)PhotonNetwork.CurrentRoom.CustomProperties[basePosition];
        Debug.Log(basePosition);
    }
}
