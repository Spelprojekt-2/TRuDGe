using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(Grappleable))]
public class HandleHandler : Editor {
    void OnSceneGUI() {
        var vis = target as Grappleable;
        if (vis == null) return;
        
        switch (Tools.current)
        {
            case Tool.Move:
            {   
                EditorGUI.BeginChangeCheck();

                Vector3 p0 = Handles.PositionHandle(vis.transform.position + vis.directionGizmoPosition, Quaternion.identity);
                Vector3 p1 = Handles.PositionHandle(vis.transform.position + vis.directionGizmoPosition + vis.desiredExitDirection, Quaternion.identity);
                
                if (EditorGUI.EndChangeCheck()) {
                    Undo.RecordObject(target, "Desired Exit Direction");

                    Vector3 newP0 = p0 - vis.transform.position;
                    if (vis.directionGizmoPosition != newP0)
                    {
                        vis.directionGizmoPosition = p0 - vis.transform.position;
                    }
                    else
                    {                 
                        vis.desiredExitDirection = p1 - p0;
                    }

                    EditorUtility.SetDirty(target);
                }
            } break;
            case Tool.Rotate:
                EditorGUI.BeginChangeCheck();
                Quaternion r = Handles.RotationHandle(Quaternion.LookRotation(vis.desiredExitDirection, vis.transform.up), vis.transform.position + vis.directionGizmoPosition);

                if (EditorGUI.EndChangeCheck()) {
                    Undo.RecordObject(target, "Desired Exit Direction");

                    float m = vis.desiredExitDirection.magnitude;
                    Vector3 newD = r * (Vector3.forward * m);
                    vis.desiredExitDirection = newD;

                    EditorUtility.SetDirty(target);
                }
                break;
            default: break;
        }

        
    }
}
#endif
public class Grappleable : MonoBehaviour, IGrappleable
{
    [SerializeField] private Vector3 grapplePoint = Vector3.zero;
    public Vector3 desiredExitDirection = Vector3.forward * 5f;
    public Vector3 directionGizmoPosition = new Vector3(5,5,5);
    [SerializeField] private float angleMargin = 10f;
    [SerializeField] private ShowGizmoEnum showGizmos = ShowGizmoEnum.Selected;
    public Vector3 GetGrapplePoint(GrapplingBehaviour gb) => transform.TransformPoint(grapplePoint);
    public bool IsLocking => false;
    public bool IsDesiredDirection(Vector3 direction)
    {
        float targetAngle = Mathf.Atan2(desiredExitDirection.z, desiredExitDirection.x);
        float currentAngle = Mathf.Atan2(direction.z, direction.x);

        if (Mathf.Abs(targetAngle - currentAngle) < angleMargin * Mathf.Deg2Rad)
        {
            // Debug.Log("targetAngle: " + targetAngle + "currentAngle: " + currentAngle);
            return true;
        }
        else return false;
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

    public void EnteredCutCollider(GameObject grapplingObject)
    {
        if (grapplingObject.TryGetComponent<GrapplingBehaviour>(out GrapplingBehaviour grapple))
        {
            grapple.EndGrapple(true);
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
        DrawExitDirection();
    }

    public void OnDrawGizmosSelected()
    {
        if (showGizmos != ShowGizmoEnum.Selected) return;

        DrawGrapplingPoint();
        DrawExitDirection();
    }

    private void DrawGrapplingPoint()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.TransformPoint(grapplePoint), 0.5f);
    }
    
    private void DrawExitDirection()
    {
        Quaternion l = Quaternion.Euler( 0f, -angleMargin, 0f);
        Quaternion r = Quaternion.Euler( 0f, angleMargin, 0f);

        Gizmos.color = Color.red;
        Vector3 p0 = transform.position + directionGizmoPosition;
        Vector3 p1 = transform.position + directionGizmoPosition + l * desiredExitDirection;
        Vector3 p2 = transform.position + directionGizmoPosition + r * desiredExitDirection;
        Vector3 p3 = transform.position + directionGizmoPosition + l * (desiredExitDirection * 0.7f + new Vector3(-desiredExitDirection.z, 0f, desiredExitDirection.x) * 0.3f);
        Vector3 p4 = transform.position + directionGizmoPosition + r * (desiredExitDirection * 0.7f + new Vector3(desiredExitDirection.z, 0f, -desiredExitDirection.x) * 0.3f);
        Gizmos.DrawLine(p0, p1);
        Gizmos.DrawLine(p0, p2);
        Gizmos.DrawLine(p1, p3);
        Gizmos.DrawLine(p2, p4);
    }
}
