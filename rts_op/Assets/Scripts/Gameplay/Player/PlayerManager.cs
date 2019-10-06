using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : NetMonoBehaviour
{
    public Basis basisBuilding;

    private void Awake()
    {
        photonView.Owner.TagObject = this;
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            GetComponent<Hotkeys>().enabled = true;
            GetComponent<Camera>().enabled = true;
            GetComponent<AudioListener>().enabled = true;
            PhotonNetwork.Instantiate("base_lvl1_with_textures", new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
