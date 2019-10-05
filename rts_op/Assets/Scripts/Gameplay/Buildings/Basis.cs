using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Building))]
public class Basis : MonoBehaviourPun
{
    public BaseData playerData;

    public void ApplyDamage(float damage)
    {
        throw new System.NotImplementedException();
    }

    void Awake()
    {
        ((PlayerManager)photonView.Owner.TagObject).basisBuilding = this;
    }

    void Start()
    {
        string basePosition = "base" + photonView.Owner.ActorNumber.ToString() + "Position"; //TODO: Turn into array
        transform.position = (Vector3)PhotonNetwork.CurrentRoom.CustomProperties[basePosition];
        Debug.Log(basePosition);
    }

    private void Update()
    {
    }
}
