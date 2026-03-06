using UnityEngine;

public class Grappleable : MonoBehaviour, IGrappleable
{
    [SerializeField] private Vector3 grapplePointOffset = Vector3.zero;
    [SerializeField] private ShowGizmoEnum showGizmos = ShowGizmoEnum.Selected;
    public Vector3 GetGrapplePoint(GrapplingBehaviour gb) => transform.TransformPoint(grapplePointOffset);
    public bool IsLocking => false;
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
        
        DrawGrapplingPoint();
    }

    public void OnDrawGizmosSelected()
    {
        if (showGizmos != ShowGizmoEnum.Selected) return;

        DrawGrapplingPoint();
    }

    private void DrawGrapplingPoint()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.TransformPoint(grapplePointOffset), 0.5f);
    }
}
