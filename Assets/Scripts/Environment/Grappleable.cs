using UnityEngine;

public class Grappleable : MonoBehaviour
{
    [SerializeField] private Vector3 grapplePointOffset = Vector3.zero;
    public Vector3 GrapplePoint => transform.position + grapplePointOffset;
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
}
