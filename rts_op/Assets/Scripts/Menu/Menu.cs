﻿using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviourPunCallbacks
{
    public float mapXMax = 900;
    public float mapYMax = 600;
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Conncted to Photon Masterserver");
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join random room");
        CreateRandomRoom();
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Mission to join random room: success");
        PhotonNetwork.LoadLevel("test-map");
    }
    public void CreateRandomRoom()
    {
        Vector3 baseCalcVector = new Vector3(Random.Range(mapXMax - 25f, 150), 7.5f, Random.Range(mapYMax - 25f, -mapYMax + 25f));
        Debug.Log("Creating random room");
        var roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 2 };
        roomOps.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable(); roomOps.CustomRoomProperties.Add("seed", Random.Range(-200000000, 200000000));
        roomOps.CustomRoomProperties.Add("base1Position", baseCalcVector);
        roomOps.CustomRoomProperties.Add("base2Position", -baseCalcVector + new Vector3(0, 15, 0));
        PhotonNetwork.CreateRoom(null, roomOps);
    }
}
