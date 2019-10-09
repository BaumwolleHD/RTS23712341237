using UnityEngine;
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

    private Vector3 GetHighestGroundPoint()
    {
        RaycastHit hit;

        Ray ray = new Ray(new Vector3(transform.position.x, 1000f, transform.position.z), Vector3.down);

        int groundLayer = 1 << 8;

        if (Physics.Raycast(ray, out hit, 2000f, groundLayer))
        {
            return hit.point;
        }
        else
        {
            Debug.LogError("No ground found for " + name);
            EditorGUIUtility.PingObject(this);
            Debug.Break();
            return Vector3.one * -1000;
        }
    }

    public void PutOnGround()
    {
        transform.position = GetHighestGroundPoint();
    }
    
    public GameObject InstanciateOnGround(string prefabName, int x, int z)
    {
        float highestY = GetHighestGroundPoint().y;

        return PhotonNetwork.Instantiate(prefabName, new Vector3(x, highestY, z), Quaternion.identity);
    }
}