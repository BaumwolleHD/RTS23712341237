using UnityEngine;
using UnityEditor;

public class PlayerMonoBehaviour : NetMonoBehaviour
{

    PlayerManager internalPlayerManager;

    public PlayerManager playerManager
    {
        get
        {
            if (internalPlayerManager)
            {
                return internalPlayerManager;
            }
            else
            {
                internalPlayerManager = GetComponentInParent<PlayerManager>();
                return playerManager;
            }

        }
    }
}