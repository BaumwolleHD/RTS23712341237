using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float gameTime;
    public MapSettings mapSettings;
    public int playerCount;

    public List<PlayerManager> allPlayers;

    bool playerSpawned;

    private void Start()
    {
        if(!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("Menu");
        }
    }
    private void Update()
    {
        if(mapIsReady && !playerSpawned)
        {
            playerSpawned = true;
            PhotonNetwork.Instantiate("Player", new Vector3(60, 70, 60), Quaternion.Euler(40, -130, 0));
        }

        if (playerCount > 1)
        {
            Timer();
        }
    }

    private bool mapIsReady { get { return GameObject.Find("Map Magic") && !GameObject.Find("Map Magic").GetComponent<MapMagic.MapMagic>().IsWorking; } }

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