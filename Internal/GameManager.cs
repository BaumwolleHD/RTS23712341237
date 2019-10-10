using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        MapMagic.MapMagic mm = FindObjectOfType<MapMagic.MapMagic>();
        mm.gameObject.SetActive(false);
    }
    void Start()
    {
    }
}
