using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerData playerData;

    private void Start()
    {
        PhotonNetwork.Instantiate("Base", new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
    }
}
