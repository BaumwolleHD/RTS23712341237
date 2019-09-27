using System.Collections.Generic;

static class ListExtensions
{
    public static T PopAt<T>(this List<T> list, int index)
    {
        T r = list[index];
        list.RemoveAt(index);
        return r;
    }
    public static T PopRandom<T>(this List<T> list) where T : class, new()
    {
        if (list.Count == 0) return null;
        int index = UnityEngine.Random.Range(0, list.Count - 1);
        T r = list[index];
        list.RemoveAt(index);
        return r;
    }
    public static T GetRandom<T>(this List<T> list) where T : class, new()
    {
        if (list.Count == 0) return null;
        int index = UnityEngine.Random.Range(0, list.Count - 1);
        T r = list[index];
        return r;
    }

}