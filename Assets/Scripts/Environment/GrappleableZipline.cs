using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrappleableZipline : MonoBehaviour, IGrappleable
{
    [SerializeField] private List<Vector3> grapplePoints = new List<Vector3>( new Vector3[3]);
    private List<float> pointTs = new List<float>( new float[3]);
    [Tooltip("The time it takes for the grapple point to move from the first point to the last point")]
    [SerializeField] private float interpolationTime = 1f;
    [Tooltip("Releases the grapple once the second point is reached")]
    [SerializeField] private bool releaseAtLastPoint = true;
    [Tooltip("Make the player unable to release the grapple manually. Has no effect if Realease At Last Point is false")]
    [SerializeField] private bool lockPlayerToGrapple = true;
    [SerializeField] private ShowGizmoEnum showGizmos = ShowGizmoEnum.Selected;
    public bool IsLocking => lockPlayerToGrapple && releaseAtLastPoint;
    public Vector3 GetGrapplePoint(GrapplingBehaviour gb)
    {
        if (grapplePoints.Count == 1) return transform.TransformPoint(grapplePoints[0]);
        
        // Determine t
        float t = gb.TimeSinceGrapple / interpolationTime;
        if (t > 1)
        {
            t = 1;
            
            if (releaseAtLastPoint) gb.EndGrapple(false);
        }

        // Binary search which points to interpolate between
        int l = 0,
            r = pointTs.Count-1,
            mid = (l + r) / 2;

        while (mid != l && mid != r)
        {
            if (t < pointTs[mid])
                r = mid;
            else
                l = mid;
            mid = (l + r) / 2;
        }

        if (l == r) Debug.LogError("Binary search bugged: l == r");

        float t0 = pointTs[l];
        float t1 = pointTs[r];

        // Interpolate
        t -= t0;
        t1 -= t0;
        t = t / t1;
        Vector3 p = Vector3.Lerp(grapplePoints[l], grapplePoints[r], t);

        Debug.Log("l: " + l + " r: " + r + " t: " + t);
        // Debug.Log(p + "" + grapplePoints[l] + "" + grapplePoints[r] + "" + t + "" + t1);
        return transform.TransformPoint(p);
    }
    public void Start()
    {
        RecalculateLength();
    }
    public void OnValidate()
    {
        RecalculateLength();
    }
    private void RecalculateLength()
    {
        // If 0 grappling points make 1
        if (grapplePoints.Count == 0) grapplePoints = new List<Vector3>( new Vector3[1]);

        // If less than 2 grappling points return
        if (grapplePoints.Count < 2) return;

        // If 2 or more grappling points prepare values for interpolation
        if (interpolationTime <= 0) interpolationTime = 1;

        pointTs = new List<float>( new float[grapplePoints.Count]);

        // If more than 2 grappling points also prepare values for the non-extreme positions
        if (grapplePoints.Count > 2)
        {    
            List<float> segmentLengths = new List<float>( new float[grapplePoints.Count-1]);
            float totalLength = 0;
            for (int i = 0; i < segmentLengths.Count; i++)
            {
                float length = Vector3.Distance(grapplePoints[i], grapplePoints[i+1]);
                segmentLengths[i] = length;
                totalLength += length;
            }
            // Debug.Log("segmentLengths -");
            // foreach (var item in segmentLengths)
            // {
            //     Debug.Log(item);
            // }
            // Debug.Log(Vector3.Distance(grapplePoints[0], grapplePoints[1]));
            // Debug.Log("total length - " + totalLength);
            float traversal = 0;
            for (int i = 1; i < segmentLengths.Count; i++)
            {
                traversal += segmentLengths[i-1];
                pointTs[i] = traversal/totalLength;
                // Debug.Log("SegmentLength: " + segmentLengths[i-1]);
                // Debug.Log("\nTraversal: " + traversal);
                // Debug.Log("\npointT: " + pointTs[i]);
            }
        }

        // Assign values to the extreme positions
        pointTs[0] = 0;
        pointTs[^1] = 1;

        // Debug.Log("t-");
        // for (int i = 0; i < pointTs.Count; i++)
        // {
        //     Debug.Log(pointTs[i]);
        // }
    }
    public void EnteredGrappleRange(GameObject grapplingObject)
    {
        if (grapplingObject.TryGetComponent<GrapplingBehaviour>(out GrapplingBehaviour grapple))
        {
            grapple.EnteredGrappleRange(this);
        }
        else
        {
            Debug.LogError("Grappling object does not have GrapplingBehaviour component!");
            return;
        }
    }

    public void ExitedGrappleRange(GameObject grapplingObject)
    {
        if (grapplingObject.TryGetComponent<GrapplingBehaviour>(out GrapplingBehaviour grapple))
        {
            grapple.ExitedGrappleRange(this);
        }
        else
        {
            Debug.LogError("Grappling object does not have GrapplingBehaviour component!");
            return;
        }
    }
    public void OnDrawGizmos()
    {
        if (showGizmos != ShowGizmoEnum.Always) return;
        
        DrawGrapplingPoints();
    }
    public void OnDrawGizmosSelected()
    {
        if (showGizmos != ShowGizmoEnum.Selected) return;

        DrawGrapplingPoints();
    }
    private void DrawGrapplingPoints()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.TransformPoint(grapplePoints[0]), 0.5f);

        Gizmos.color = Color.blue;
        for (int i = 1; i < grapplePoints.Count; i++)
        {
            Gizmos.DrawSphere(transform.TransformPoint(grapplePoints[i]), 0.5f);
            Gizmos.DrawLine(transform.TransformPoint(grapplePoints[i-1]), transform.TransformPoint(grapplePoints[i]));
        }
    }
}
