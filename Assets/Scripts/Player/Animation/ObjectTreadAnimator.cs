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
            Gizmos.DrawWireSphere(worldPos, wheel.radius);
        }
        
        Gizmos.color = Color.magenta;
        Vector3 v0 = wheels[0].localPosition - wheels[1].localPosition;
        // Vector3 v1 = new Vector3
    }
}