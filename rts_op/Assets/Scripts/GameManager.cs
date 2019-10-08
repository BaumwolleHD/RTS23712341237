using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float gameTime;
    public MapSettings mapSettings;
    public bool playerSpawned;

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
    private void Update()
    {
        if (playerSpawned)
        {
            Timer();
        }
    }
    void Timer()
    {
        gameTime += Time.deltaTime;
    }
}


[CreateAssetMenu(fileName = "Data", menuName = "MapSettings")]
public class MapSettings : ScriptableObject
{
    public Material[] teamMaterials;
    public float mapXMax = 900;
    public float mapYMax = 600;
}