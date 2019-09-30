using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        PhotonNetwork.Instantiate("Player", new Vector3(60,70,60), Quaternion.Euler(40,-130,0));
    }
}
