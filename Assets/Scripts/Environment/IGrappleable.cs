using UnityEngine;

public interface IGrappleable
{
    public Vector3 GetGrapplePoint(GrapplingBehaviour gb);
    public bool IsLocking { get; }
    public void EnteredGrappleRange(GameObject grapplingObject);
    public void ExitedGrappleRange(GameObject grapplingObject);
}