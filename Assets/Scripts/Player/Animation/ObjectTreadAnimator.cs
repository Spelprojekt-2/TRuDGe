using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct Wheel
{
    public Vector3 localPosition;
    public float radius;
}
public class ObjectTreadAnimator : MonoBehaviour
{
    struct TreadSpline
    {
        private TreadSplineWheel[] wheels;
        public float totalLength
        {
            get
            {
                return wheels[^1].distanceFromStart + wheels[^1].length;
            }
        }
        public TreadSpline(List<Wheel> wheelData, float innerTreadMargin, float outerTreadMargin)
        {
            wheels = new TreadSplineWheel[wheelData.Count];
            float distanceFromStart = 0f;

            // Get the end point of the last wheel to start the loop
            float r0 = wheelData[^1].radius + innerTreadMargin;
            float r1 = wheelData[0].radius + innerTreadMargin;
            Vector3 diff = wheelData[0].localPosition - wheelData[^1].localPosition;
            Vector3 dir = diff.normalized;
            float dist = diff.magnitude;

            float lineLength = TreadSplineWheel.TwoCircleTangentLength(r0, r1, dist);
            float theta = TreadSplineWheel.TwoCircleTangentAngle(r0, r1, dist);

            Vector3 tangentPointDir = TreadSplineWheel.RotateVectorOnX( dir, theta );
            Vector3 tangentPointVector = tangentPointDir * r0;
            Vector3 lineDir = new Vector3(0, tangentPointDir.z, -tangentPointDir.y);
            Vector3 lineStart = wheelData[^1].localPosition + tangentPointVector;
            Vector3 prevEndPoint = lineStart + lineDir * lineLength;

            // Now create all wheels in order
            for (int i = 0; i < wheelData.Count; i++)
            {
                int ip1 = (i + 1) % wheelData.Count;
                wheels[i] = new TreadSplineWheel(
                    wheelData[i].localPosition,
                    wheelData[i].radius + innerTreadMargin,
                    prevEndPoint,
                    wheelData[ip1].localPosition,
                    wheelData[ip1].radius + innerTreadMargin,
                    distanceFromStart
                );
                distanceFromStart += wheels[i].length;
                prevEndPoint = wheels[i].endPoint;
            }
        }
        public Quaternion GetRotation(float splineDistance)
        {
            if (splineDistance < 0f)
                splineDistance = 0f;
            else if (splineDistance > totalLength)
                splineDistance = totalLength;
            return wheels[GetAppropriateSegmentIndex(splineDistance)].GetRotation(splineDistance);
        }
        public Vector3 GetLocation(float splineDistance)
        {
            if (splineDistance < 0f)
                splineDistance = 0f;
            else if (splineDistance > totalLength)
                splineDistance = totalLength;
            return wheels[GetAppropriateSegmentIndex(splineDistance)].GetLocation(splineDistance);
        }
        private int GetAppropriateSegmentIndex(float splineDistance)
        {
            int min = 0;
            int max = wheels.Length - 1;
            while (min < max)
            {
                int mid = (min + max) / 2;
                if (splineDistance < wheels[mid].distanceFromStart)
                {
                    max = mid - 1;
                }
                else if (splineDistance >= wheels[mid].distanceFromStart)
                {
                    if (splineDistance < wheels[mid + 1].distanceFromStart)
                    {
                        return mid;
                    }
                    min = mid + 1;
                }
            }
            return min;
        }
        public void DrawGizmos()
        {
            foreach (TreadSplineWheel wheel in wheels)
            {
                wheel.DrawGizmos();
            }
        }
        struct TreadSplineWheel
        {
            public Vector3 center { get; private set; }
            public float radius { get; private set; }
            public float distanceFromStart { get; private set; }
            public float length => arcLength + lineLength;
            public Vector3 endPoint => lineEnd;
            private float lineLength;
            private Vector3 lineStart;
            private Vector3 lineDir;
            private Vector3 lineEnd => lineStart + lineDir * lineLength;
            private float arcStartAngle;
            private float arcSpanAngle;
            private float arcLength => arcSpanAngle * radius;
            private Vector3 prevEndPoint;
            
            public TreadSplineWheel(Vector3 center, float radius, Vector3 prevEndPoint, Vector3 nextCenter, float nextRadius, float distanceFromStart)
            {
                this.center = center;
                this.radius = radius;
                this.distanceFromStart = distanceFromStart;
                arcStartAngle = 0f;
                arcSpanAngle = 0f;
                lineLength = 0f;
                lineStart = Vector3.zero;
                lineDir = Vector3.zero;

                // Calculate the tangent points of this and the next wheel
                Vector3 diff = nextCenter - center;
                Vector3 dir = diff.normalized;
                float dist = diff.magnitude;

                lineLength = TwoCircleTangentLength(radius, nextRadius, dist);
                float theta = TwoCircleTangentAngle(radius, nextRadius, dist);

                Vector3 tangentPointDir = RotateVectorOnX( dir, theta );
                Vector3 tangentPointVector = tangentPointDir * radius;
                lineDir = new Vector3(0, tangentPointDir.z, -tangentPointDir.y);

                lineStart = center + tangentPointVector;

                this.prevEndPoint = prevEndPoint;

                // Calculate arc angles
                Vector3 toPrev = prevEndPoint - center;
                arcStartAngle = -Mathf.Atan2(toPrev.z, toPrev.y);
                arcSpanAngle = Vector2.SignedAngle(
                    new Vector2(toPrev.z, toPrev.y),
                    new Vector2(tangentPointVector.z, tangentPointVector.y)
                ) * Mathf.Deg2Rad;
            }
            public void Move(Vector3 newCenter)
            {
                center = newCenter;
            }
            public Vector3 GetLocation(float distance)
            {
                distance -= distanceFromStart;
                if (distance < arcLength)
                {
                    float angleOfPoint = (distance / arcLength * arcSpanAngle) + arcStartAngle;
                    return center + RotateVectorOnX(new Vector3(0, radius, 0), angleOfPoint);
                }
                else
                {
                    distance -= arcLength;
                    return lineStart + lineDir * distance;
                }
            }
            public Quaternion GetRotation(float distance)
            {
                distance -= distanceFromStart;
                if (distance < arcLength)
                {
                    // Wheel rotation
                    float angleOfPoint = (distance / arcLength * arcSpanAngle) + arcStartAngle;
                    Vector3 v = RotateVectorOnX(new Vector3(0, 1, 0), angleOfPoint);
                    return Quaternion.LookRotation(
                        new Vector3(0, v.z, -v.y),
                        v
                    );
                }
                else
                {
                    distance -= arcLength;
                    // Line segment rotation
                    Vector3 up = new Vector3(0, -lineDir.z, lineDir.y);
                    return Quaternion.LookRotation(lineDir, up);
                }
            }
            public void DrawGizmos()
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(center, center + RotateVectorOnX(new Vector3(0, 2, 0), arcStartAngle));
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(center, center + RotateVectorOnX(new Vector3(0, 2, 0), arcStartAngle + arcSpanAngle));
            }
            public static float TwoCircleTangentLength(float r1, float r2, float d)
            {
                return Mathf.Sqrt(d * d - (r1 - r2) * (r1 - r2));
            }
            public static float TwoCircleTangentAngle(float r1, float r2, float d)
            {
                float tangent = TwoCircleTangentLength(r1, r2, d);
                float hypotenuse = Mathf.Sqrt(tangent * tangent + r2 * r2); 
                return -Mathf.Acos((r1 * r1 + d * d - hypotenuse * hypotenuse) / (2 * r1 * d));
            }
            public static Vector3 RotateVectorOnX(Vector3 v, float angle)
            {
                float cos = Mathf.Cos(angle);
                float sin = Mathf.Sin(angle);
                return new Vector3(
                    v.x,
                    v.y * cos + v.z * sin,
                    v.y * -sin + v.z * cos
                );
            }
        }
    }
    [SerializeField] private List<Wheel> wheels = new List<Wheel>();
    [SerializeField] private float innerTreadMargin = 0.2f;
    [SerializeField] private float outerTreadMargin = 0.2f;

    [Header("Gizmos")]
    [SerializeField] private ShowGizmoEnum showGizmos = ShowGizmoEnum.Always;

    private TreadSpline treadSpline;

    void OnValidate()
    {
        treadSpline = new TreadSpline(wheels, innerTreadMargin, outerTreadMargin);
    }
    private void OnDrawGizmos()
    {
        if (showGizmos == ShowGizmoEnum.Always)
            DrawGizmos();
    }
    private void OnDrawGizmosSelected()
    {
        if (showGizmos == ShowGizmoEnum.Selected)
            DrawGizmos();
    }
    private void DrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Transform t = transform;
        foreach (Wheel wheel in wheels)
        {
            Vector3 worldPos = t.position + t.rotation * wheel.localPosition;
            DrawCircle(worldPos, t.right, wheel.radius, 32, Color.cyan);
        }
        
        Gizmos.color = Color.magenta;
        
        float segmentLength = 0.2f;
        for (float d = segmentLength; d < treadSpline.totalLength; d += segmentLength)
        {
            Gizmos.color = Color.magenta;
            Vector3 point0 = treadSpline.GetLocation(d - segmentLength);
            Vector3 point1 = treadSpline.GetLocation(d);
            Gizmos.DrawLine(t.TransformPoint(point0), t.TransformPoint(point1));

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(t.TransformPoint(point0), t.TransformPoint(point0 + treadSpline.GetRotation(d - segmentLength) * Vector3.up * segmentLength));

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(t.TransformPoint(point0), t.TransformPoint(point0 + treadSpline.GetRotation(d - segmentLength) * Vector3.forward * segmentLength/2));
        }

        treadSpline.DrawGizmos();


        // Vector3 v0 = wheels[1].localPosition - wheels[0].localPosition;
        // float dist0 = v0.magnitude;
        // float tangent0 = TwoCircleTangentLength(wheels[0].radius, wheels[1].radius, dist0);
        // Vector3 v0n = v0.normalized;
        // Gizmos.DrawLine(t.TransformPoint(wheels[0].localPosition), t.TransformPoint(wheels[1].localPosition));


        // float angle0 = TwoCircleTangentAngle(wheels[0].radius, wheels[1].radius, v0.magnitude);

        // Vector3 tangentPointVector0 = RotateVectorOnX( v0n, -angle0 ) * wheels[0].radius;
        // Vector3 rv0n = RotateVectorOnX( v0n, -angle0 );
        // Gizmos.DrawLine(
        //     t.TransformPoint(
        //         wheels[0].localPosition + tangentPointVector0
        //     ),
        //     t.TransformPoint(
        //         wheels[0].localPosition + tangentPointVector0 + new Vector3(0, rv0n.z, -rv0n.y) * tangent0
        //     )
        //     );

        // Vector3 lastLinePoint = Vector3.zero;
        // for (int i = 0; i < wheels.Count; i++)
        // {
        //     int ip1 = (i + 1) % wheels.Count; 

        //     float r0 = wheels[i].radius + innerTreadMargin;
        //     float r1 = wheels[ip1].radius + innerTreadMargin;

        //     Vector3 diff = wheels[ip1].localPosition - wheels[i].localPosition;
        //     Vector3 dir = diff.normalized;
        //     float dist = diff.magnitude;

        //     float tangent = TwoCircleTangentLength(r0, r1, dist);
        //     float theta = TwoCircleTangentAngle(r0, r1, dist);

        //     Vector3 tangentPointDir = RotateVectorOnX( dir, theta );
        //     Vector3 tangentPointVector = tangentPointDir * r0;
        //     Vector3 tangentDir = new Vector3(0, -tangentPointDir.z, tangentPointDir.y);

        //     Vector3 lineP0 = wheels[i].localPosition + tangentPointVector;
        //     Vector3 lineP1 = wheels[i].localPosition + tangentPointVector + tangentDir * tangent;

        //     // Draw connecting curve
        //     // if (i != 0)
        //     // {
        //     //     float arcAngle0 = Vector3.SignedAngle(lastLinePoint - wheels[i].localPosition, Vector3.forward, Vector3.right);
        //     //     float arcAngle1 = Vector3.SignedAngle(lineP0 - wheels[i].localPosition, Vector3.forward, Vector3.right);

        //     //     DrawArc(
        //     //         t.TransformPoint(wheels[i].localPosition),
        //     //         t.right,
        //     //         r0,
        //     //         arcAngle0 * Mathf.Deg2Rad,
        //     //         arcAngle1 * Mathf.Deg2Rad,
        //     //         16,
        //     //         Color.magenta
        //     //     );
        //     // }

        //     // Draw tangent line of both wheels
        //     Gizmos.DrawLine(
        //         t.TransformPoint( lineP0 ),
        //         t.TransformPoint( lineP1 )
        //     );
        // }
    }

    #region Gizmos Helpers
    private void DrawCircle(Vector3 center, Vector3 normal, float radius, int segments, Color color)
    {
        Gizmos.color = color;

        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
        Vector3 prevPoint = center + rotation * new Vector3(radius, 0, 0);
        for (int i = 1; i <= segments; i++)
        {
            float angle = (float)i / segments * Mathf.PI * 2f;
            Vector3 newPoint = center + rotation * new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            Gizmos.DrawLine(prevPoint, newPoint);
            prevPoint = newPoint;
        }
    }
    private void DrawArc(Vector3 center, Vector3 normal, float radius, float startAngle, float endAngle, int segments, Color color)
    {
        Gizmos.color = color;

        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
        Vector3 prevPoint = center + rotation * new Vector3(Mathf.Cos(startAngle) * radius, 0, Mathf.Sin(startAngle) * radius);
        for (int i = 1; i <= segments; i++)
        {
            float angle = Mathf.Lerp(startAngle, endAngle, (float)i / segments);
            Vector3 newPoint = center + rotation * new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            Gizmos.DrawLine(prevPoint, newPoint);
            prevPoint = newPoint;
        }
    }
    #endregion

    #region Math Helpers
    private float TwoCircleTangentLength(float r1, float r2, float d)
    {
        return Mathf.Sqrt(d * d - (r1 - r2) * (r1 - r2));
    }
    private float TwoCircleTangentAngle(float r1, float r2, float d)
    {
        float tangent = TwoCircleTangentLength(r1, r2, d);
        float hypotenuse = Mathf.Sqrt(tangent * tangent + r2 * r2); 
        return Mathf.Acos((r1 * r1 + d * d - hypotenuse * hypotenuse) / (2 * r1 * d));
    }
    private Vector3 RotateVectorOnX(Vector3 v, float angle)
    {
        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);
        return new Vector3(
            v.x,
            v.y * cos + v.z * sin,
            v.y * -sin + v.z * cos
        );
    }
    #endregion
}