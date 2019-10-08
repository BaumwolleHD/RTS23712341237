﻿using UnityEngine;
using UnityEditor;
using Photon.Pun;

public class NetMonoBehaviour : MonoBehaviourPun
{
    public void Destroy()
    {
        if (photonView.isRuntimeInstantiated)
        {
            if(photonView.IsMine)
            {
                PhotonNetwork.Destroy(photonView);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public PlayerManager ownerPlayerManager
    {
        get
        {
            return ((PlayerManager)photonView.Owner.TagObject);
        }
    }

    public GameManager gameManager
    {
        get
        {
            return FindObjectOfType<GameManager>();
        }
    }

    /// <summary>
    /// Player number of the player owning this GameObject (starts with 1!)
    /// </summary>
    public int OwnerActorNumber
    {
        get
        {
            if(!Application.isPlaying)
            {
                return 1;
            }
            return photonView.Owner.ActorNumber;
        }
    }

    public void AddObservedComponent(Component component)
    {
        photonView.ObservedComponents.Add(component);
        if (!photonView.ObservedComponents[0]) photonView.ObservedComponents.RemoveAt(0);
        photonView.Synchronization = ViewSynchronization.Unreliable;
    }

    public bool hasOwner
    {
        get { return photonView.Owner != null; }
    }
}