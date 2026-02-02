using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class TreadSuspAnimator : MonoBehaviour
{
    private Transform t;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private string blendShapePrefix = "";
    [SerializeField] private List<Vector3> wheelPositions = new List<Vector3>();
    private List<Vector3> hitPositions = new List<Vector3>();
    private int suspendedWheelCount = 4;

    [SerializeField][Range(0,0.5f)] private float minSuspensionDistance = 0.2f;
    [SerializeField][Range(0,0.5f)] private float maxSuspensionDistance = 0.5f;
    [SerializeField] private float lossyScale;
    [SerializeField] private bool showGizmos = false;

    private void Awake()
    {
        t = GetComponent<Transform>();
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        suspendedWheelCount = wheelPositions.Count;
    }

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
        if (hitPositions.Count != suspendedWheelCount)
        {
            hitPositions = new List<Vector3>(new Vector3[suspendedWheelCount]);
        }
    }

    void Update()
    {
        lossyScale = t.lossyScale.y;
        for (int i = 0; i < suspendedWheelCount; i++)
        {
            Vector3 worldPos = t.position + t.rotation * wheelPositions[i];
            Ray ray = new Ray(worldPos + t.up * 0.5f * t.lossyScale.y, -t.up);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, maxSuspensionDistance + minSuspensionDistance, groundLayer))
            {
                float distance = hitInfo.distance;
                float blendShapeValue = distance * 100f / t.lossyScale.y;
                skinnedMeshRenderer.SetBlendShapeWeight(i, blendShapeValue);
                hitPositions[i] = hitInfo.point;
            }
            else
            {
                skinnedMeshRenderer.SetBlendShapeWeight(i, 100f);
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        if (!showGizmos) return;
        Gizmos.color = Color.blue;
        foreach (Vector3 localPos in wheelPositions)
        {
            Vector3 worldPos = t.position + t.rotation * localPos;
            Gizmos.DrawLine(worldPos + t.up * minSuspensionDistance, worldPos - t.up * maxSuspensionDistance);
            Gizmos.DrawSphere(worldPos, 0.05f);
        }
        foreach (Vector3 hitPos in hitPositions)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hitPos, 0.07f);
        }
    }
}
