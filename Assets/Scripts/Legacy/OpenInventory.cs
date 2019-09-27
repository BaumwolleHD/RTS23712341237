using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenInventory : MonoBehaviour
{
    public bool showIventory;
    Vector3 inventoryScale;
    private PlayerMovement playerMovement;

    void Start()
    {
        showIventory = false;
        inventoryScale = transform.localScale;
        transform.localScale = new Vector3(0f, 0f, 0f);
        playerMovement = GetComponentInParent<PlayerMovement>();
        playerMovement.lockCursor = true;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!showIventory)
            {
                showIventory = true;
            }
            else
            {
                showIventory = false;
            }

        }
        if (showIventory)
        {
            transform.localScale = inventoryScale;
            Cursor.visible = true;
            playerMovement.lockCursor = false;
            playerMovement.disableLookAround = true;
        }
        else
        {
            transform.localScale = new Vector3(0f, 0f, 0f);
            Cursor.visible = false;
            playerMovement.lockCursor = true;
            playerMovement.disableLookAround = false;
        }
    }
}
