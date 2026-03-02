using UnityEngine;

public class Grappleable : MonoBehaviour
{
    [SerializeField] protected Vector3 grapplePointOffset = Vector3.zero;
    [SerializeField] protected ShowGizmoEnum showGizmos = ShowGizmoEnum.Selected;
    public virtual Vector3 GetGrapplePoint(GrapplingBehaviour gb) => transform.TransformPoint(grapplePointOffset);
    public virtual void EnteredGrappleRange(GameObject grapplingObject)
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

    public virtual void ExitedGrappleRange(GameObject grapplingObject)
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

    public virtual void OnDrawGizmos()
    {
        if (showGizmos != ShowGizmoEnum.Always) return;
        
        DrawGrapplingPoints();
    }

    public virtual void OnDrawGizmosSelected()
    {
        if (showGizmos != ShowGizmoEnum.Selected) return;

        DrawGrapplingPoints();
    }

    protected virtual void DrawGrapplingPoints()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.TransformPoint(grapplePointOffset), 0.5f);
    }
}
