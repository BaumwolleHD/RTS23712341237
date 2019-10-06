using UnityEngine;
using UnityEditor;
using Photon.Pun;

public class Hotkeys : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            PhotonNetwork.Instantiate("Trooper", Vector3.one, Quaternion.identity);
        }
    }
}