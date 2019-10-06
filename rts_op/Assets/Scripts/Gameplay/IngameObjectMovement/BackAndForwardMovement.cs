using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackAndForwardMovement : MonoBehaviour
{
    public float objectMovementSpeed = 30f;
    public Vector3 objectMovement;
    private Vector3 currentObjectMovement;
    private Vector3 startPosition;
    private float objectMovementSpeedPerFrame;

    private void Start()
    {
        startPosition = transform.position;
        currentObjectMovement = objectMovement;
        objectMovementSpeedPerFrame = objectMovementSpeed * Time.deltaTime;
    }

    void Update()
    {
        if (Vector3.Distance(startPosition, transform.position) >= objectMovement.magnitude)
        {
            currentObjectMovement = -objectMovement;
        }
        if (Vector3.Distance(startPosition + objectMovement, transform.position) >= objectMovement.magnitude)
        {
            currentObjectMovement = objectMovement;
        }
        transform.position += objectMovementSpeedPerFrame * currentObjectMovement;
    }
}