using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float cameraSpeed = 10;
    void Update()
    {
        if (Input.mousePosition.x <= 1)
        {
            transform.localPosition += new Vector3(-cameraSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.mousePosition.x >= Screen.width - 1)
        {
            transform.localPosition += new Vector3(cameraSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.mousePosition.y <= 1)
        {
            transform.localPosition += new Vector3(0, -Mathf.Cos(-130) * Mathf.Sin(40) * cameraSpeed * Time.deltaTime, 0);
        }
        if (Input.mousePosition.y >= Screen.height - 1)
        {
            transform.localPosition += new Vector3(0, Mathf.Sin(-130) *cameraSpeed * Time.deltaTime, Mathf.Sin(40) * cameraSpeed * Time.deltaTime); 
        }
    }
}

