using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clap : NetMonoBehaviour
{
    public float rotationSpeed = 30f;
    public Axis rotaionAxis;

    bool previousMooveWayOne;
    float rotationSpeedPerFrame;

    public BackAndForwardMovement backAndForwardMovement;

    private void Start()
    {
        previousMooveWayOne = backAndForwardMovement.mooveWayOne;
    }

    void Update()
    {
        

        switch (rotaionAxis)
        {
            case Axis.x:
                if (previousMooveWayOne != backAndForwardMovement.mooveWayOne)
                {
                    rotationSpeed = -rotationSpeed;
                    previousMooveWayOne = backAndForwardMovement.mooveWayOne;
                }
                rotationSpeedPerFrame = rotationSpeed * Time.deltaTime;
                transform.Rotate(rotationSpeedPerFrame, 0, 0);
                break;
            case Axis.y:
                if (previousMooveWayOne != backAndForwardMovement.mooveWayOne)
                {
                    rotationSpeed = -rotationSpeed;
                    previousMooveWayOne = backAndForwardMovement.mooveWayOne;
                }
                rotationSpeedPerFrame = rotationSpeed * Time.deltaTime;
                transform.Rotate(0, rotationSpeedPerFrame, 0);
                break;
            case Axis.z:
                if (previousMooveWayOne != backAndForwardMovement.mooveWayOne)
                {
                    rotationSpeed = -rotationSpeed;
                    previousMooveWayOne = backAndForwardMovement.mooveWayOne;
                }
                rotationSpeedPerFrame = rotationSpeed * Time.deltaTime;
                transform.Rotate(0, 0, rotationSpeedPerFrame);
                break;

        }
    }
}
