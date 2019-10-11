using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]

public class Building : NetMonoBehaviour
{
    public bool isBuilt {get {return remainingBuildTime < 0;}}

    [ReadOnly]
    public float remainingBuildTime;

    public float buildTime = 2f;

    void Start()
    {
        remainingBuildTime = buildTime;
    }

    void Update()
    {
        remainingBuildTime -= Time.deltaTime;
    }
}