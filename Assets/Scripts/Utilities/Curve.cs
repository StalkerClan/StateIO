using UnityEngine;

public class Curve : MonoBehaviour
{ 
    // Linear
    public static Vector3[] GetLinearBezierCurve(float totalTime, Vector3 p0, Vector3 p1)
    {
        int segmentCount = (int)(totalTime / 0.02f);
        Vector3[] positions = new Vector3[segmentCount];
        for (int i = 1; i <= segmentCount; ++i)
        {
            positions[i - 1] = CalculateLinearBezierPoint(i / (float)segmentCount, p0, p1);
        }
        return positions;
    }

    private static Vector3 CalculateLinearBezierPoint(float t, Vector3 p0, Vector3 p1)
    {
        Vector3 p = p0 * (1 - t);
        p += t * p1;
        return p;
    }

    // Quadratic
    public static Vector3[] GetQuadraticBezierCurve(float totalTime, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        int segmentCount = (int)(totalTime / 0.02f);
        Vector3[] positions = new Vector3[segmentCount];
        for (int i = 1; i <= segmentCount; ++i)
        {
            positions[i - 1] = CalculateQuadraticBezierPoint(i / (float)segmentCount, p0, p1, p2);
        }
        return positions;
    }

    private static Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float uu = u * u;
        float tt = t * t;

        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
    }

    // Cubic
    public static Vector3[] GetCubicBezierCurve(float totalTime, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        int segmentCount = (int)(totalTime / 0.02f);
        Vector3[] positions = new Vector3[segmentCount];
        for (int i = 1; i <= segmentCount; ++i)
        {
            positions[i - 1] = CalculateCubicBezierPoint(i / (float)segmentCount, p0, p1, p2, p3);
        }
        return positions;
    }

    private static Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }
}