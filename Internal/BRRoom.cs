using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

//Zum beispiel penis oder so

public class BRRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static BRRoom instance;

    void Awake()
    {
        instance = this;
    }

    void IInRoomCallbacks.OnMasterClientSwitched(Player newMasterClient)
    {
    }

    void IInRoomCallbacks.OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Player joined room");
    }

    void IInRoomCallbacks.OnPlayerLeftRoom(Player otherPlayer)
    {
        var playerManager = otherPlayer.TagObject as PlayerManager;
        if (playerManager)
        {
            Debug.Log("Left player found");
            playerManager.TryKill();

            //TODO: Clean his objects (Unimportant)
        }
    }

    void IInRoomCallbacks.OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
    }

    void IInRoomCallbacks.OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
    }

    private Camera tempCamera;

    public override void OnJoinedRoom()
    {
        Invoke("SpawnPlayer", 0.1f);
        MapMagic.MapMagic mm = Resources.FindObjectsOfTypeAll<MapMagic.MapMagic>()[0] as MapMagic.MapMagic;
        Debug.Log(PhotonNetwork.CurrentRoom.CustomProperties.ToStringFull());
        PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("seed", out object result);

        mm.seed = (int)result;
        mm.ClearResults();
        GameObject tempCameraGO = new GameObject("Temp Camera");
        tempCamera = tempCameraGO.AddComponent<Camera>();
        tempCamera.transform.position = new Vector3(-2000,250,2000);
        mm.gameObject.SetActive(true);
        mm.Generate(true);

        if(PhotonNetwork.IsMasterClient)
        {
            mm.generateRange = 1000;
        }

        
        
        Debug.Log("Joined the room");
    }

    void SpawnPlayer()
    {
        var newPlayer = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity).GetComponent<PlayerManager>();

        newPlayer.TakeControl();

        MapMagic.MapMagic mm = Resources.FindObjectsOfTypeAll<MapMagic.MapMagic>()[0] as MapMagic.MapMagic;
        if (SpawnPoint.activeSpawnPoints.Count == 0)
        {
            Debug.Log("Is still working");
            Invoke("spawnPlayer", 0.1f);
            return;
        }
        Destroy(tempCamera.gameObject);

    }

    static public List<PlayerManager> GetAllLivingPlayers()
    {
        return GetAllPlayers().Where(PM => !PM.healthManagement.isDead).ToList();
    }

    static public List<PlayerManager> GetAllPlayers()
    {
        return new List<PlayerManager>(FindObjectsOfType<PlayerManager>());
    }
}
