using UnityEngine;
using Photon.Pun;
using System;
using Photon.Realtime;
using ExitGames.Client.Photon;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class AdvancedMonoBehaviour : MonoBehaviourPun
{
    public void Invoke(Action theDelegate, float time)
    {
        MonoBehaviourExtensions.Invoke(this, theDelegate, time);
    }

    public void Cooldown(Cooldown cooldown)
    {
        if(!cooldown.IsOnCooldown)
        {
            this.StartCoroutine(MonoBehaviourExtensions.StartCooldownRoutine(this, cooldown));
        }
    }
    public void Cooldown(Cooldown cooldown, Action finalAction)
    {
        this.StartCoroutine(MonoBehaviourExtensions.StartCooldownRoutine(this, cooldown, finalAction));
    }
    public void Cooldown(Cooldown cooldown, float customCooldown)
    {
        cooldown.cooldownLength = () => customCooldown;
        this.StartCooldownRoutine(cooldown);
    }
}

[RequireComponent(typeof(PhotonView))]
public abstract class SyncedMonoBehaviour : AdvancedMonoBehaviour, IPunObservable, IPunOwnershipCallbacks, IInRoomCallbacks
{
    public bool FirstStateUpdateReceived { get; private set; }

    public bool IsMine { get { return photonView.IsMine; } }

    public int ViewID { get { return photonView.ViewID; } }

    public PlayerManager GetLocalPlayer()
    {
        return PhotonNetwork.LocalPlayer.TagObject as PlayerManager;
    }

    public static bool IsMasterClient { get { return PhotonNetwork.IsMasterClient; } }

    private bool becomeOwnerSend = false;

    public virtual void OnBecomeResponsible() {}

    public virtual void Deserialize(PhotonStream stream, PhotonMessageInfo info)
    {

    }

    public virtual void AfterDeserializedFirstTime()
    {

    }
    public virtual void Serialize(PhotonStream stream, PhotonMessageInfo info)
    {

    }

    void SendOnBecomeResponsible()
    {
        if(!becomeOwnerSend)
        {
            OnBecomeResponsible();
            becomeOwnerSend = true;
            PhotonNetwork.RemoveCallbackTarget(this);
        }
    }

    public bool IsResponsible { get { return !photonView || photonView.IsMine || (photonView.IsSceneView && IsMasterClient); } }

    public void Start()
    {
        if(IsResponsible)
        {
            SendOnBecomeResponsible();
            StartResponsible();
        }
        else
        {
            StartNotOwned();
        }
        if(photonView.Owner != null)
        {
            StartHasOwner();
        }
    }

    public void Update()
    {
        if (!IsResponsible)
        {
            UpdateNotResponsible();
        }
        else
        {
            UpdateResponsible();
            SendOnBecomeResponsible();
        }
    }

    public void FixedUpdate()
    {
        if (!IsResponsible)
        {
            FixedUpdateNotResponsible();
        }
        else
        {
            FixedUpdateResponsible();
        }
    }

    public void OnGUI()
    {
        if (!IsResponsible)
        {
            OnGUINotResponsible();
        }
        else
        {
            OnGUIResponsible();
        }
    }

    public virtual void FixedUpdateNotResponsible() { }
    public virtual void FixedUpdateResponsible() { }

    public virtual void UpdateNotResponsible() { }
    public virtual void UpdateResponsible() { }

    public virtual void OnGUINotResponsible() { }
    public virtual void OnGUIResponsible() { }

    public virtual void StartNotOwned() { }
    public virtual void StartResponsible() { }
    public virtual void StartHasOwner() { }



    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            Serialize(stream, info);
        }
        else
        {
            Deserialize(stream, info);
            if (!FirstStateUpdateReceived)
            {
                AfterDeserializedFirstTime();
            }
            FirstStateUpdateReceived = true;
        }
    }

    void IPunOwnershipCallbacks.OnOwnershipRequest(PhotonView targetView, Player requestingPlayer) { }

    void IPunOwnershipCallbacks.OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        if(targetView.ViewID == this.photonView.ViewID)
        {
            SendOnBecomeResponsible();
        }
        Debug.Log("OnOwnershipTransfered works!!");
    }

    void IInRoomCallbacks.OnPlayerEnteredRoom(Player newPlayer)
    {
    }

    void IInRoomCallbacks.OnPlayerLeftRoom(Player otherPlayer)
    {
    }

    void IInRoomCallbacks.OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
    }

    void IInRoomCallbacks.OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
    }

    void IInRoomCallbacks.OnMasterClientSwitched(Player newMasterClient)
    {
        if(newMasterClient == PhotonNetwork.LocalPlayer)
        {
            SendOnBecomeResponsible();
        }
    }


    public void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void RPC(string methodName, Player targetPlayer, params object[] parameters)
    {
        for (int i = 0; i < parameters.Length; i++)
        {
            MonoBehaviourPun asScript = (parameters[i] as MonoBehaviourPun);
            if (asScript)
            {
                parameters[i] = asScript.photonView.ViewID;
            }
            PhotonView asView = (parameters[i] as PhotonView);
            if (asView)
            {
                parameters[i] = asView.ViewID;
            }
        }
        this.photonView.RPC(methodName, targetPlayer, parameters);
    }
    public void RPC(string methodName, RpcTarget target, params object[] parameters)
    {
        for(int i = 0; i < parameters.Length; i++)
        {
            MonoBehaviourPun asScript = (parameters[i] as MonoBehaviourPun);
            if (asScript)
            {
                parameters[i] = asScript.photonView.ViewID;
            }
            PhotonView asView = (parameters[i] as PhotonView);
            if (asView)
            {
                parameters[i] = asView.ViewID;
            }
        }
        this.photonView.RPC(methodName, target, parameters);
    }

    public void TransferOwnership(int newOwnerId)
    {
        photonView.TransferOwnership(newOwnerId);
    }

    public void TransferOwnership(PlayerManager PM)
    {
        photonView.TransferOwnership(PM.ViewID);
    }

    public static T Find<T>(int ViewID) where T: SyncedMonoBehaviour
    {
        if(ViewID==0)
        {
            return null;
        }
        PhotonView view = PhotonView.Find(ViewID);
        if(!view)
        {
            return default(T);
        }
        T t = view.GetComponent<T>();
        return t;
    }
}



public abstract class PlayerMonoBehaviour : SyncedMonoBehaviour
{
    private PlayerManager PMCache;
    public PlayerManager PM {
        get
        {
            if(!PMCache)
            {
                PMCache = FindPM();
            }
            return PMCache;
        }
    }
    private PlayerManager FindPM()
    {
        PlayerManager result = this as PlayerManager;
        if (result) return result;

        result = GetComponent<PlayerManager>();
        if (result) return result;
        
        result = GetComponentInParent<PlayerManager>();
        if (result) return result;

        result = GetComponentInChildren<PlayerManager>();
        if (result) return result;
#if UNITY_EDITOR
        EditorGUIUtility.PingObject(this);
#endif
        Debug.LogError("Failed to find PlayerManger for class " + this.GetType().Name + " of GameObject " + this.transform.name);
        return null;
    }

    public virtual void StartWhenSpectating() {}

    public virtual void OnGUIWhenSpectating() {}

    public bool IsSpectatedPlayer()
    {
        if (!PM)
        {
            Debug.Log(PM.transform.name);
        }
        return PM.playerCamera.enabled;
    }


    public new void Start()
    {
        base.Start();
        PlayerManager temp = PM;
    }

    public override void StartResponsible()
    {
        base.StartResponsible();
        if (IsSpectatedPlayer())
        {
            StartWhenSpectating();
        }
    }

    public override void OnGUIResponsible()
    {
        base.OnGUIResponsible();
        if (IsSpectatedPlayer())
        {
            OnGUIWhenSpectating();
        }
    }
}