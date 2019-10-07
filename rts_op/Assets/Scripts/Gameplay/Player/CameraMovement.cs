using System;
using Unity.Collections;
using UnityEngine;

public class CameraMovement : NetMonoBehaviour
{
    public float cameraSpeed = 10;
    public float zoomSpeed = 30;
    public float rotationSpeed = 30;
    public int movementMargin = 10;
    private Quaternion startRotation;
    Vector2 lastMousePosition;

    private void Start()
    {
        startRotation = transform.localRotation;
    }

    void Update()
    {
        if (Menu.instance.isPaused)
        {
            return;
        }
        
        Zoom();

        MarginMovement();

        RotateAroundCenterOfView();

        if (Input.GetKeyDown(KeyCode.E))
        {
            LookAtBase();
        }
    }

    public void LookAtBase()
    {
        transform.position = playerManager.basisBuilding.transform.position + new Vector3(60, 70, 60);
        transform.localRotation = startRotation;
    }

    private void RotateAroundCenterOfView()
    {
        Ray ray = GetComponent<Camera>().ScreenPointToRay(new Vector3(Screen.width/2f,Screen.height/2f));
        RaycastHit hit;
        Vector3 pos = Vector3.zero;
        if (Physics.Raycast(ray, out hit))
        {
            pos = hit.point;
        }
        Vector2 rotationSkalator = new Vector2(rotationSpeed * (lastMousePosition.x - Input.mousePosition.x), rotationSpeed * (lastMousePosition.y - Input.mousePosition.y));    
        
        if (Input.GetMouseButton(2))
        {
            transform.RotateAround(pos, Vector3.up, rotationSkalator.x);

            Vector3 arschFick = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * Vector3.left;

            transform.RotateAround(pos, arschFick, -rotationSkalator.y);
        }
        lastMousePosition = Input.mousePosition;
    }

    private void Zoom()
    {
        float ScrollWheelChange = Input.GetAxis("Mouse ScrollWheel");

        float R = ScrollWheelChange * zoomSpeed;  //This little peece of code is written by JelleWho https://github.com/jellewie
        float PosX = transform.eulerAngles.x + 90;              //Get up and down
        float PosY = -1 * (transform.eulerAngles.y - 90);       //Get left to right
        PosX = PosX / 180 * Mathf.PI;                                       //Convert from degrees to radians
        PosY = PosY / 180 * Mathf.PI;                                       //^
        float X = R * Mathf.Sin(PosX) * Mathf.Cos(PosY);                    //Calculate new coords
        float Z = R * Mathf.Sin(PosX) * Mathf.Sin(PosY);                    //^
        float Y = R * Mathf.Cos(PosX);
        transform.localPosition = transform.localPosition + new Vector3(X, Y, Z);//Move the main camera
    }

    private void MarginMovement()
    {
        if (Input.mousePosition.x <= movementMargin || Input.GetKey(KeyCode.A))
        {
            transform.localPosition += Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * new Vector3(-cameraSpeed * GetCameraHeight() * Time.deltaTime, 0, 0);
        }
        if (Input.mousePosition.x >= Screen.width - movementMargin || Input.GetKey(KeyCode.D))
        {
            transform.localPosition += Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * new Vector3(cameraSpeed * GetCameraHeight() * Time.deltaTime, 0, 0);
        }
        if (Input.mousePosition.y <= movementMargin || Input.GetKey(KeyCode.S))
        {
            transform.localPosition += Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * new Vector3(0, 0, -cameraSpeed * GetCameraHeight() * Time.deltaTime);
        }
        if (Input.mousePosition.y >= Screen.height - movementMargin || Input.GetKey(KeyCode.W))
        {
            transform.localPosition += Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * new Vector3(0, 0, cameraSpeed * GetCameraHeight() * Time.deltaTime);
        }
    }

    float GetCameraHeight()
    {
        return transform.localPosition.y;
    }
}

