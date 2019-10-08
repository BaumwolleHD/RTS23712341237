using UnityEngine;
using UnityEditor;
using Photon.Pun;

public class Hotkeys : MonoBehaviour
{
    public Unit unitToSpawn;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            PhotonNetwork.Instantiate(unitToSpawn.name, Vector3.one, Quaternion.identity);
        }
    }
}