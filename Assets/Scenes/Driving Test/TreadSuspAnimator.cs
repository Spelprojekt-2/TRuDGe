using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class TreadSuspAnimator : MonoBehaviour
{
    private Transform t;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private string blendShapePrefix = "";
    [SerializeField] private List<Vector3> wheelPositions = new List<Vector3>();
    private int suspendedWheelCount = 4;

    [SerializeField][Range(0,0.5f)] private float minSuspensionDistance = 0.2f;
    [SerializeField][Range(0,0.5f)] private float maxSuspensionDistance = 0.5f;

    void Start()
    {
        t = GetComponent<Transform>();
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        suspendedWheelCount = wheelPositions.Count;
    }

    void OnValidate()
    {
        t = GetComponent<Transform>();
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        suspendedWheelCount = wheelPositions.Count;
    }

    void Update()
    {
        for (int i = 0; i < suspendedWheelCount; i++)
        {
            Vector3 worldPos = t.TransformPoint(wheelPositions[i]);
            Ray ray = new Ray(worldPos + t.up * 0.5f, -t.up);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, maxSuspensionDistance + minSuspensionDistance))
            {
                float distance = hitInfo.distance;
                float blendShapeValue = distance * 100f;
                skinnedMeshRenderer.SetBlendShapeWeight(i, blendShapeValue);
            }
            else
            {
                skinnedMeshRenderer.SetBlendShapeWeight(i, 100f);
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        foreach (Vector3 localPos in wheelPositions)
        {
            Vector3 worldPos = t.TransformPoint(localPos);
            Gizmos.DrawLine(worldPos + t.up * minSuspensionDistance, worldPos - t.up * maxSuspensionDistance);
            Gizmos.DrawSphere(worldPos, 0.05f);
        }
    }
}
