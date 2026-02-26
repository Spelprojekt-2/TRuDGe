using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class TreadAnimator : MonoBehaviour
{
    [System.Serializable]
    public struct Wheel
    {
        public Transform wheelObject;
        public Vector3 restPosition;
        public Vector3 rayCastPosition;
        public Wheel(Transform wheelObject, Vector3 restPosition, Vector3 rayCastPosition)
        {
            this.wheelObject = wheelObject;
            this.restPosition = restPosition;
            this.rayCastPosition = rayCastPosition;
        }
        public void SetSuspensionHeight(float height)
        {
            wheelObject.localPosition = restPosition - new Vector3( 0, height, 0);
        }
    }
    private SkinnedMeshRenderer skinnedMeshRenderer;
    [Header("Texture")]
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Rigidbody vehicleRigidBody;
    [SerializeField] float xLocationOverride;
    [SerializeField] bool reverseTextureDirection = false;
    [SerializeField] bool YToXDirection = true;
    [Header("Suspension")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private List<Wheel> wheels = new List<Wheel>();
    private Vector3[] hitPositions = new Vector3[3];
    [SerializeField] private float minSuspensionDistance = 0.3f;
    [SerializeField][Range(-100,0)] private float minBlendShapeValue = -100f;
    [SerializeField] private float maxSuspensionDistance = 0.3f;
    [SerializeField][Range(0,100)] private float maxBlendShapeValue = 100f;
    [Header("Gizmos")]
    [SerializeField] private ShowGizmoEnum showGizmos = ShowGizmoEnum.Never;
    void Start()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
    }
    void Update()
    {
        // Texture
        float velocity = playerMovement.GetCurrentSpeed(false);
        velocity += vehicleRigidBody.angularVelocity.y * xLocationOverride;
        velocity *= Time.deltaTime;
        if (reverseTextureDirection) velocity = - velocity;
        Vector2 deltaOffset = new Vector2(0, velocity);
        if (YToXDirection) deltaOffset = new Vector2(velocity, 0);

        Vector2 offset = skinnedMeshRenderer.material.mainTextureOffset;
        offset = new Vector2(
            (offset.x + deltaOffset.x) % 1,
            (offset.y + deltaOffset.y) % 1);
        skinnedMeshRenderer.material.mainTextureOffset = offset;

        // Suspension
        float lossyScale = transform.lossyScale.y;
        Vector3[] hits = new Vector3[wheels.Count];
        for (int i = 0; i < wheels.Count; i++)
        {
            Vector3 worldPos = transform.TransformPoint(wheels[i].rayCastPosition);
            Ray ray = new Ray(worldPos + transform.up * 0.5f * lossyScale, -transform.up);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, maxSuspensionDistance + minSuspensionDistance, groundLayer))
            {
                float distance = hitInfo.distance - 0.5f * lossyScale;

                wheels[i].SetSuspensionHeight(distance);

                float blendShapeValue = Mathf.Lerp(
                    minBlendShapeValue, maxBlendShapeValue,
                    Mathf.InverseLerp(-minSuspensionDistance, maxSuspensionDistance, distance)
                );
                skinnedMeshRenderer.SetBlendShapeWeight(i, blendShapeValue);
                hits[i] = hitInfo.point;
            }
            else
            {
                wheels[i].SetSuspensionHeight(maxSuspensionDistance);
                skinnedMeshRenderer.SetBlendShapeWeight(i, maxBlendShapeValue);
            }
        }
        hitPositions = hits;
    }
    void OnDrawGizmos()
    {
        if (showGizmos == ShowGizmoEnum.Always) DrawGizmos();
    }
    void OnDrawGizmosSelected()
    {
        if (showGizmos == ShowGizmoEnum.Selected) DrawGizmos();
    }
    void DrawGizmos()
    {
        foreach (Wheel wheel in wheels)
        {
            Gizmos.color = Color.blue;
            Vector3 worldPos = transform.TransformPoint(wheel.rayCastPosition);
            Gizmos.DrawLine(worldPos + transform.up * minSuspensionDistance, worldPos - transform.up * maxSuspensionDistance);
            Gizmos.DrawWireSphere(worldPos, 0.05f);
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.TransformPoint(wheel.restPosition), 0.05f);
        }
        Gizmos.color = Color.red;
        foreach (Vector3 hitPos in hitPositions)
        {
            Gizmos.DrawSphere(hitPos, 0.07f);
        }
    }
}
