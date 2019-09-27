using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

//Eine Random generierte Scheiße, wo man random Waffen finden kann und sich gegenseitig abknallt.(Das ist nichtmal ein Satzt) - Marion 2019

/// <summary>
/// Was macht spaß?
/// 1. Herausforderung
/// 2. Fortschritt (z.B. LP in lol)
/// 3. Überraschung (unerwartetes muss passieren)
/// </summary>

//IDEE:
//1. Man Spawnt random
//2. Man sammelt Waffen etc.
//3. Man trifft ein Spieler und es gibt einen Kampf

    //TODO: Make players push each other or not

public class PlayerMovement : PlayerMonoBehaviour
{
    public float speed = 100f;
    public float ladderSpeed = 20f;
    public float jumpForce = 6f;
    Rigidbody rb;

    Vector2 _mouseAbsolute;
    Vector2 _smoothMouse;

    public Vector2 clampInDegrees = new Vector2(360, 180);
    public bool lockCursor;
    public Vector2 sensitivity = new Vector2(2, 2);
    public Vector2 smoothing = new Vector2(3, 3);
    public Vector2 targetDirection;
    public Vector2 targetCharacterDirection;
    public bool disableLookAround;

    [HideInInspector] public PlayerHead head;
    [HideInInspector] private Camera Cam;

    [ReadOnly] public bool isOnLadder;



    void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();
        head = GetComponentInChildren<PlayerHead>();
    }

    new void Start()
    {
        base.Start();
        Cursor.visible = false;

        Debug.Log("Jasper ist toll");

        //TODO: Investigate bug where rotation slowsly increases

        // Ensure the cursor is always locked when set
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        targetDirection = head.transform.localRotation.eulerAngles;
        targetCharacterDirection = head.transform.localRotation.eulerAngles;
    }

    new void Update()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    new void FixedUpdate()
    {
        CalculateMovement();
        LockXZ();
    }

    void LockXZ() //TOOD: Check if rigidbody can do this
    {
        if(PM.healthManagement.currentHealth > 0f)
        {
            PM.model.transform.rotation = Quaternion.Euler(0f, PM.model.transform.rotation.eulerAngles.y, 0f);
        }
    }

    void CalculateMovement()
    {
        CalculateMouseMovement();
        CalculateKeyMovement();
    }

    void CheckForShooting()
    {
        if(PM.weapon)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PM.weapon.RequestShootSingle(PM);
            }
            if (Input.GetMouseButton(0))
            {
                PM.weapon.RequestShootAuto(PM);
            }
        }
    }

    private void LookAround()
    {
        
        // Allow the script to clamp based on a desired target value.
        var targetOrientation = Quaternion.Euler(targetDirection);
        var targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);

        // Get raw mouse input for a cleaner reading on more sensitive mice.
        var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        // Scale input against the sensitivity setting and multiply that against the smoothing value.
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));

        // Interpolate mouse movement over time to apply smoothing delta.
        _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
        _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / smoothing.y);

        // Find the absolute mouse movement value from point zero.
        _mouseAbsolute += _smoothMouse;

        // Clamp and apply the local x value first, so as not to be affected by world transforms.
        if (clampInDegrees.x < 360)
            _mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);

        // Then clamp and apply the global y value.
        if (clampInDegrees.y < 360)
            _mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);

        head.transform.localRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right) * targetOrientation;

        Quaternion yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, head.transform.InverseTransformDirection(Vector3.up));
        head.transform.localRotation *= yRotation;


        float headY = head.transform.localRotation.eulerAngles.y;

        head.transform.localRotation = Quaternion.Euler(head.transform.localRotation.eulerAngles.x, 0, head.transform.localRotation.eulerAngles.z);

        PM.model.transform.localRotation = Quaternion.Euler(PM.model.transform.localRotation.eulerAngles.x, headY, PM.model.transform.localRotation.eulerAngles.z);

        PM.model.WeaponSocket.eulerAngles = new Vector3(PM.playerCamera.transform.eulerAngles.x, PM.playerCamera.transform.eulerAngles.y, PM.playerCamera.transform.eulerAngles.z);
    }

    void CalculateMouseMovement()
    {
        CheckForShooting();
        if (!disableLookAround)
        {
            LookAround();
        }
    }

    [HideInInspector]
    public bool ladderHasLeftGround = false;

    void CalculateKeyMovement()
    {
        Vector3 Movement;
        if (!isOnLadder)
        {
            Movement = Quaternion.AngleAxis(head.transform.rotation.eulerAngles.y, Vector3.up) * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * speed;
            Movement *= Time.fixedDeltaTime * 10;
            rb.velocity = new Vector3(Movement.x, rb.velocity.y, Movement.z);
        }
        else
        {
            if(ladderHasLeftGround)
            {
                if(PM.model.CanJump(out Cooldown cddd))
                {
                    isOnLadder = false;
                }
            }
            else
            {
                if(!PM.model.CanJump(out Cooldown cdddd))
                {
                    ladderHasLeftGround = true;
                }
            }
            Movement = Quaternion.AngleAxis(head.transform.rotation.eulerAngles.y, Vector3.up) * new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * speed;
            Movement *= Time.fixedDeltaTime * 10;
            if(Input.GetAxis("Vertical") < 0 && PM.model.CanJump(out Cooldown cdd))
            {
                rb.velocity = new Vector3(Movement.x, 0, Movement.z);
            }
            else
            {
                rb.velocity = new Vector3(Movement.x, Input.GetAxis("Vertical") * ladderSpeed, Movement.z);
            }
        }
        if (Input.GetKey(KeyCode.Space) && PM.model.CanJump(out Cooldown cd))
        {
            Cooldown(cd);
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            PM.TryDropWeapon();
        }
        if (Input.GetKey(KeyCode.R))
        {
            PM.TryReload();
        }
    }
}