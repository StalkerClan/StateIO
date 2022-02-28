using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class Extensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        for (var i = 0; i < list.Count; i++)
            list.Swap(i, UnityEngine.Random.Range(i, list.Count));
    }

    public static void Swap<T>(this IList<T> list, int i, int j)
    {
        T temp = list[i];
        list[i] = list[j];
        list[j] = temp;
    }

    public static int LastIndex<T>(this IList<T> list)
    {
        return list.Count - 1;
    }

    public static T LastItem<T>(this IList<T> list)
    {
        return list[list.Count - 1];
    }

    public static T GetRandomItem<T>(this IList<T> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static void ResetTransform(this Transform trans)
    {
        trans.position = Vector3.zero;
        trans.localRotation = Quaternion.identity;
        trans.localScale = Vector3.one;
    }

    private static CultureInfo ci = new CultureInfo("en-us");
    public static string IntToCurrencyString(this int value)
    {
        return value.ToString("N00", ci);
    }
}
