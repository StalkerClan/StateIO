using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMonoBehaviour : MonoBehaviour
{
    private Transform _transform;
    public new Transform transform { get { if (_transform == null) _transform = GetComponent<Transform>(); return _transform; } }
    [HideInInspector]
    public bool inPool = false;

    protected float deltaTime { get { return Time.deltaTime; } }
    protected float fixedDeltaTime { get { return Time.fixedDeltaTime; } }

    protected float MaxX { get { return Camera.main.orthographicSize * Camera.main.aspect; } }

    protected float MinX { get { return -MaxX; } }

    protected float MaxY { get { return Camera.main.orthographicSize; } }

    protected float MinY { get { return -MaxY; } }

    protected float BottomEdge { get { return MinY - 0.3f; } }

    protected float TopEdge { get { return MaxY + 0.3f; } }

    protected List<T> EnumToList<T>()
    {
        Type enumType = typeof(T);

        // You can't use type constraints on value types, so have to check & throw error.
        if (enumType.BaseType != typeof(Enum))
            throw new ArgumentException("T must be of type System.Enum type");

        Array enumValArray = Enum.GetValues(enumType);

        List<T> enumValList = new List<T>(enumValArray.Length);

        foreach (int val in enumValArray)
        {
            enumValList.Add((T)Enum.Parse(enumType, val.ToString()));
        }

        return enumValList;
    }

    protected string[] EnumToStringArray<T>()
    {
        return Enum.GetNames(typeof(T));
    }

    protected float RadianToDegree(float radian)
    {
        return radian * 180 / Mathf.PI;
    }

    protected float DegreeToRadian(float degree)
    {
        return degree * Mathf.PI / 180;
    }

    protected Vector2 VectorByRotateAngle(float angle, Vector2 v) // Unity quay theo chiều ngược kim đồng hồ
    {
        float cos = Mathf.Cos(DegreeToRadian(angle));
        float sin = Mathf.Sin(DegreeToRadian(angle));

        return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
    }

    protected Vector3 VectorByRotateAngle(float angle, Vector3 v) // Unity quay theo chiều ngược kim đồng hồ
    {
        float cos = Mathf.Cos(DegreeToRadian(angle));
        float sin = Mathf.Sin(DegreeToRadian(angle));

        return new Vector3(v.x * cos - v.y * sin, v.x * sin + v.y * cos, 0);
    }
}
