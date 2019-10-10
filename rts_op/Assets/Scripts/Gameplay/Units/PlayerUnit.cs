﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Unit))]
public class PlayerUnit : NetMonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Unit>().unitOwner == ownerPlayerManager && !Menu.instance.isPaused) WalkToMouse();

        IEnumerator e = DrawPath(GetComponent<NavMeshAgent>().path);
        StartCoroutine(e);
    }
    
    private Color c = Color.white;

    void WalkToMouse()
    {
        if (!Camera.main)
        {
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction, Color.blue, 1f);
        RaycastHit hit;
        Vector3 pos = Vector3.zero;

        int groundLayer = 1 << 8;

        if (Physics.Raycast(ray, out hit, 1000f, groundLayer))
        {
            if(Vector3.Distance(GetComponent<NavMeshAgent>().destination, pos)>0.1f)
            {
                GetComponent<Unit>().WalkTo(hit.point);
            }            
        }
        

    }

    IEnumerator DrawPath(NavMeshPath path)
    {
        yield return new WaitForEndOfFrame();
        if (path.corners.Length >= 2)
        {
            switch (path.status)
            {
                case NavMeshPathStatus.PathComplete:
                    c = Color.white;
                    break;
                case NavMeshPathStatus.PathInvalid:
                    c = Color.red;
                    break;
                case NavMeshPathStatus.PathPartial:
                    c = Color.yellow;
                    break;
            }

            Vector3 previousCorner = path.corners[0];

            int i = 1;
            while (i < path.corners.Length)
            {
                Vector3 currentCorner = path.corners[i];
                Debug.DrawLine(previousCorner, currentCorner, c, 0.1f);
                previousCorner = currentCorner;
                i++;
            }
        }

    }
}
