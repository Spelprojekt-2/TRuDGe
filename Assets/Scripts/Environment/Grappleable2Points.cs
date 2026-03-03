using UnityEngine;

public class Grappleable2Points : Grappleable
{
    [SerializeField] protected Vector3 secondGrapplePointOffset = Vector3.zero;
    [Tooltip("The time it takes for the grapple point to move from the first point to the second point")]
    [SerializeField] protected float interpolationTime = 1f;
    [SerializeField] protected bool cutAtPoint2 = true;
    public override Vector3 GetGrapplePoint(GrapplingBehaviour gb)
    {
        float t = gb.TimeSinceGrapple / interpolationTime;
        if (t > 1)
        {
            t = 1;
            
            if (cutAtPoint2) gb.EndGrapple();
        }

        return transform.TransformPoint(Vector3.Lerp(grapplePointOffset, secondGrapplePointOffset, t));
    }

    protected override void DrawGrapplingPoints()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.TransformPoint(grapplePointOffset), 0.5f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.TransformPoint(secondGrapplePointOffset), 0.5f);
        Gizmos.DrawLine(transform.TransformPoint(grapplePointOffset), transform.TransformPoint(secondGrapplePointOffset));
    }
}
