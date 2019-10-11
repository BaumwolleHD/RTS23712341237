using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]

public class NeutralUnit : UnitMonoBehaviour
{
    private void Start()
    {
        unit.WalkTo(transform.position + (Vector3.one * Random.Range(0.2f, 2f)));
    }
}
