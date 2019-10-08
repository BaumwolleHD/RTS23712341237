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
            GetComponent<PlayerManager>().SpawnUnit(unitToSpawn);
        }
    }
}