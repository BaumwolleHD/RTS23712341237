using ExitGames.Client.Photon;
using MapMagic;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetMonoBehaviour
{
    public float gameSpeed = 1;

    public float gameTime;
    public MapSettings mapSettings;
    public int playerCount;

    public List<PlayerManager> allPlayers;
    public bool spawnAI = true;

    bool playerSpawned;

    private void Awake()
    {
        PhotonPeer.RegisterType(typeof(Damageable), (byte)'E', Damageable.Serialize, Damageable.Deserialize);
    }
    private void Start()
    {
        if(!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("Menu");
        }
    }
    private void Update()
    {
        Time.timeScale = gameSpeed;

        if (!PhotonNetwork.IsConnected) return;
        if(mapIsReady && !playerSpawned)
        {
            playerSpawned = true;
            Debug.Log("Spawning player");
            PhotonNetwork.Instantiate("Player", new Vector3(60, 70, 60), Quaternion.Euler(40, -130, 0));
            if(spawnAI) PhotonNetwork.Instantiate("AIPlayer", Vector3.zero, Quaternion.identity);
        }

        if (playerCount > 1)
        {
            Timer();
        }
    }

    private bool mapIsReady { get { return !ThreadWorker.IsWorking("MapMagic");} }

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