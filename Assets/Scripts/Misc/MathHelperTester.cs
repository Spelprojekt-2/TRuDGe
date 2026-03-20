using UnityEngine;

public class MathHelperTester : MonoBehaviour
{
    [SerializeField] Vector3 point;
    [SerializeField] Vector3 axis;
    [SerializeField] float angle;


    void OnDrawGizmos()
    {
        // Draw target
        Gizmos.color = Color.magenta;

        Gizmos.DrawSphere(point, 0.5f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.up);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.TransformDirection(axis));

        axis.Normalize();
        Vector3 localSpacePoint = transform.InverseTransformPoint(point);
        Vector3 pointOnAxisPlane = localSpacePoint - axis * Vector3.Dot(localSpacePoint, axis);

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.TransformPoint(pointOnAxisPlane), 0.5f);

        Vector3 crossForward = Vector3.Cross(transform.right, transform.TransformDirection(axis));
        Gizmos.DrawLine(transform.position, transform.position + crossForward);
        Gizmos.DrawLine(transform.position, transform.position + transform.TransformDirection(pointOnAxisPlane));

        angle = Vector3.SignedAngle(crossForward, transform.TransformDirection(pointOnAxisPlane), transform.TransformDirection(axis));

        Gizmos.color = Color.magenta;

        Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(angle, transform.TransformDirection(axis))* crossForward);
        
        transform.GetChild(0).localRotation = MathHelpers.PointAtGlobalPointOnLocalAxisRotation(transform, point, axis);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.GetChild(0).position, transform.GetChild(0).position + transform.GetChild(0).right);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.GetChild(0).position, transform.GetChild(0).position + transform.GetChild(0).up);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.GetChild(0).position, transform.GetChild(0).position + transform.GetChild(0).forward);
        
        // // Draw target
        // Gizmos.color = Color.magenta;

        // Gizmos.DrawSphere(point, 0.5f);

        // Gizmos.color = Color.red;
        // Gizmos.DrawLine(transform.position, transform.position + transform.right);

        // Gizmos.color = Color.green;
        // Gizmos.DrawLine(transform.position, transform.position + transform.up);

        // Gizmos.color = Color.blue;
        // Gizmos.DrawLine(transform.position, transform.position + transform.forward);

        // Gizmos.color = Color.yellow;
        // Gizmos.DrawLine(transform.position, transform.position + transform.TransformDirection(axis));

        // axis.Normalize();
        // Vector3 localSpacePoint = transform.InverseTransformPoint(point);
        // Vector3 pointOnAxisPlane = localSpacePoint - axis * Vector3.Dot(localSpacePoint, axis);

        // Gizmos.color = Color.cyan;
        // Gizmos.DrawSphere(transform.TransformPoint(pointOnAxisPlane), 0.5f);

        // Vector3 crossForward = Vector3.Cross(transform.right, transform.TransformDirection(axis));
        // Gizmos.DrawLine(transform.position, transform.position + crossForward);
        // Gizmos.DrawLine(transform.position, transform.position + transform.TransformDirection(pointOnAxisPlane));

        // angle = Vector3.SignedAngle(crossForward, transform.TransformDirection(pointOnAxisPlane), transform.TransformDirection(axis));

        // Gizmos.color = Color.magenta;

        // Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(angle, transform.TransformDirection(axis))* crossForward);
        
    }
}
