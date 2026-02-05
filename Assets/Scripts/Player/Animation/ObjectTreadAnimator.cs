using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct Wheel
{
    public Vector3 localPosition;
    public float radius;
}
public class ObjectTreadAnimator : MonoBehaviour
{
    [SerializeField] private List<Wheel> wheels = new List<Wheel>();

    [Header("Gizmos")]
    [SerializeField] private ShowGizmoEnum showGizmos = ShowGizmoEnum.Always;

    private void OnDrawGizmos()
    {
        if (showGizmos == ShowGizmoEnum.Always)
            DrawGizmos();
    }
    private void OnDrawGizmosSelected()
    {
        if (showGizmos == ShowGizmoEnum.Selected)
            DrawGizmos();
    }
    private void DrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Transform t = transform;
        foreach (Wheel wheel in wheels)
        {
            Vector3 worldPos = t.position + t.rotation * wheel.localPosition;
            DrawCircle(worldPos, t.right, wheel.radius, 32, Color.cyan);
        }
        
        Gizmos.color = Color.magenta;
        Vector3 v0 = wheels[0].localPosition - wheels[1].localPosition;
        Vector3 v1 = new Vector3(v0.x, -v0.z, v0.y).normalized;
        Gizmos.DrawLine(t.TransformPoint(wheels[0].localPosition + v1), t.TransformPoint(wheels[1].localPosition + v1));
    }
    private void DrawCircle(Vector3 center, Vector3 normal, float radius, int segments, Color color)
    {
        Gizmos.color = color;

        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
        Vector3 prevPoint = center + rotation * new Vector3(radius, 0, 0);
        for (int i = 1; i <= segments; i++)
        {
            float angle = (float)i / segments * Mathf.PI * 2f;
            Vector3 newPoint = center + rotation * new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            Gizmos.DrawLine(prevPoint, newPoint);
            prevPoint = newPoint;
        }
    }

    #region Math Helpers
    private float TwoCircleTangentLength(float r1, float r2, float d)
    {
        return Mathf.Sqrt(d * d - (r1 - r2) * (r1 - r2));
    }
    #endregion
}