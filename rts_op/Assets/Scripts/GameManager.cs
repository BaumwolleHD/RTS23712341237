using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        if(!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("Menu");
        }
        else
        {
            PhotonNetwork.Instantiate("Player", new Vector3(60, 70, 60), Quaternion.Euler(40, -130, 0));
        }
    }
}
