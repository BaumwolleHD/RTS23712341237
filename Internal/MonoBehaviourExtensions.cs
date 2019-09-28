using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

public static class MonoBehaviourExtensions
{
    public static IEnumerator StartCooldownRoutine(this MonoBehaviour me, Cooldown cooldown, Action finalAction)
    {
        Cooldown.CooldownInternal.SetIsOnCooldown(cooldown, true);
        yield return new WaitForSeconds(cooldown.cooldownLength());
        Cooldown.CooldownInternal.SetIsOnCooldown(cooldown, false);
        finalAction();
    }

    public static IEnumerator StartCooldownRoutine(this MonoBehaviour me, Cooldown cooldown)
    {
        Cooldown.CooldownInternal.SetIsOnCooldown(cooldown, true);
        yield return new WaitForSeconds(cooldown.cooldownLength());
        Cooldown.CooldownInternal.SetIsOnCooldown(cooldown, false);
    }

    public static void Invoke(this MonoBehaviour me, Action theDelegate, float time)
    {
        me.StartCoroutine(ExecuteAfterTime(theDelegate, time));
    }

    public static IEnumerator ExecuteAfterTime(Action theDelegate, float delay)
    {
        yield return new WaitForSeconds(delay);
        theDelegate();
    }

}

[System.Serializable]
public class Cooldown
{
    public bool IsOnCooldown { get; private set;}
    public Func<float> cooldownLength;
    public Cooldown(Func<float> cooldownLength)
    {
        this.cooldownLength = cooldownLength;
    }
    public Cooldown(float cooldownLength)
    {
        this.cooldownLength = ()=>cooldownLength;
    }

    public static class CooldownInternal
    {
        public static void SetIsOnCooldown(Cooldown me, bool isOnCooldown)
        {
            me.IsOnCooldown = isOnCooldown;
        }
    }
}