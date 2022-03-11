using UnityEngine;

public static class Helpers
{
    public static Vector2 PerpendicularClockwise(Vector2 vector2)
    {
        return new Vector2(vector2.y, -vector2.x);
    }

    public static Vector2 PerpendicularCounterClockwise(Vector2 vector2)
    {
        return new Vector2(-vector2.y, vector2.x);
    }

    public static Vector2 VectorByRotateAngle(float angle, Vector2 v) // Unity quay theo chiều ngược kim đồng hồ
    {
        float cos = Mathf.Cos(DegreeToRadian(angle));
        float sin = Mathf.Sin(DegreeToRadian(angle));

        return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
    }

    public static Vector3 VectorByRotateAngle(float angle, Vector3 v) // Unity quay theo chiều ngược kim đồng hồ
    {
        float cos = Mathf.Cos(DegreeToRadian(angle));
        float sin = Mathf.Sin(DegreeToRadian(angle));

        return new Vector3(v.x * cos - v.y * sin, v.x * sin + v.y * cos, 0);
    }

    public static float RadianToDegree(float radian)
    {
        return radian * 180 / Mathf.PI;
    }

    public static float DegreeToRadian(float degree)
    {
        return degree * Mathf.PI / 180;
    }
}
