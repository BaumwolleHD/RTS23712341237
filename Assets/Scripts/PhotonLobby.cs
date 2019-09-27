using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    static PhotonLobby instance;

    private void Awake()
    {
        if(instance)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }
    
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = false;
    }
    

    void Update()
    {

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log(cause.ToString());
        Debug.Log("here");
        PhotonNetwork.OfflineMode = true;
    }
    
    public override void OnConnectedToMaster()

    {
        Debug.Log("Conncted to Photon Masterserver");
        OnBattleButtonClicked();
    }

    public void OnBattleButtonClicked()
    {
        Debug.Log("Battle button clicked");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join random room");
        CreateRandomRoom();
    }

    public static void RestartGame()
    {
        //PhotonNetwork.Disconnect();
        //SceneManager.LoadScene("Map");
    }

    public void CreateRandomRoom()
    {
        Debug.Log("Creating random room");
        var roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 100};
        roomOps.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() {{ "seed", Random.Range(-200000000, 200000000) }};
         
        roomOps.CleanupCacheOnLeave = false;
        PhotonNetwork.CreateRoom(null, roomOps);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room created successfully!");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Create room failed");
    }
    
    public void OnCancelButtonClicked()
    {
        Debug.Log("Canceling...");
        PhotonNetwork.LeaveRoom();
    }

    public void RegisterHeadSpan(int viewId)
    {
        //Check that not already exists
    }

    private void OnDestroy()
    {
        Debug.Log("Lobby destroyed");
    }
}
