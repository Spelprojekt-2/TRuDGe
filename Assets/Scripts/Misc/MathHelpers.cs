using UnityEngine;

public static class MathHelpers
{
    #region Rotation
    public static Quaternion PointAtGlobalPointOnLocalAxisRotation(Transform transform, Vector3 point, Vector3 axis)
    {
        axis.Normalize();
        Vector3 localSpacePoint = transform.InverseTransformPoint(point);
        Vector3 pointOnAxisPlane = localSpacePoint - axis * Vector3.Dot(localSpacePoint, axis);

        Vector3 crossForward = Vector3.Cross(Vector3.right, axis);

        float angle = Vector3.SignedAngle(crossForward, pointOnAxisPlane, axis);

        
        return Quaternion.AngleAxis(angle, axis);
    }

    #endregion
}