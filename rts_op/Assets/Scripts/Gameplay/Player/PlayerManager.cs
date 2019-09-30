﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviourPun
{
    public PlayerData playerData;

    private void Start()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.Instantiate("Base", new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
        }
    }
}
