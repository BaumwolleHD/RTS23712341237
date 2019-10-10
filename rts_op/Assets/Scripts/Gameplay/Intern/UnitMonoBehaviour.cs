using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMonoBehaviour : NetMonoBehaviour
{
    Unit parentInternal;
    Damageable damageableInternal;

    public Unit unit
    {
        get
        {
            if (parentInternal)
            {
                return parentInternal;
            }
            else
            {
                parentInternal = GetComponentInParent<Unit>();
                return unit;
            }

        }
    }

    public Damageable damageable
    {
        get
        {
            if (damageableInternal)
            {
                return damageableInternal;
            }
            else
            {
                damageableInternal = GetComponentInParent<Damageable>();
                return damageableInternal;
            }

        }
    }


}
