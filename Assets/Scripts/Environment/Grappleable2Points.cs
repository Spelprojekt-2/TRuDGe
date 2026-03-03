using UnityEngine;

public class Grappleable2Points : Grappleable
{
    [SerializeField] protected Vector3 secondGrapplePointOffset = Vector3.zero;
    [Tooltip("The time it takes for the grapple point to move from the first point to the second point")]
    [SerializeField] protected float interpolationTime = 1f;
    [Tooltip("Releases the grapple once the second point is reached")]
    [SerializeField] protected bool releaseAtPoint2 = true;
    [Tooltip("Make the player unable to release the grapple manually. Has no effect if Realease At Point 2 is false")]
    [SerializeField] protected bool lockPlayerToGrapplePoint = true;
    public override bool IsLocking => lockPlayerToGrapplePoint && releaseAtPoint2;
    public override Vector3 GetGrapplePoint(GrapplingBehaviour gb)
    {
        float t = gb.TimeSinceGrapple / interpolationTime;
        if (t > 1)
        {
            t = 1;
            
            if (releaseAtPoint2) gb.EndGrapple(false);
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
