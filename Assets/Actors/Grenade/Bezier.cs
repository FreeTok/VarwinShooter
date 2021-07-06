using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier : MonoBehaviour
{
    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        Vector3 p01 = Vector3.Lerp(p0, p1, t);
        Vector3 p12 = Vector3.Lerp(p1, p2, t);

        Vector3 p012 = Vector3.Lerp(p01, p12, t);

        return p012;
    }

    public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        t = Mathf.Clamp01(t);
        float OneMinusT = 1f - t;
        return
               //             3f * OneMinusT * OneMinusT * (p1 - p0) +
               //             6f * OneMinusT * t * (p1 - p2) +
               //             3f * t * t * (p3 - p2);

            3f * OneMinusT * OneMinusT * (p1 - p0) +
            3f * t * t * (p2 - p1);
    }
}
