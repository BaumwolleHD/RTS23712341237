using UnityEngine.AI;

public class UnitMonoBehaviour : NetMonoBehaviour
{
    Unit parentInternal;
    Damageable damageableInternal;
    NavMeshAgent pathfinderInternal;

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

    protected NavMeshAgent pathfinder
    {
        get
        {
            if (pathfinderInternal)
            {
                return pathfinderInternal;
            }
            else
            {
                pathfinderInternal = GetComponentInParent<NavMeshAgent>();
                return pathfinderInternal;
            }

        }
    }


}
