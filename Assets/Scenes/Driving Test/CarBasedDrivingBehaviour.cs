using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(CarBasedDrivingBehaviour))]
public class CarBasedDrivingBehaviourEditor : Editor
{
    public void OnSceneGUI()
    {
        CarBasedDrivingBehaviour behaviour = (CarBasedDrivingBehaviour)target;
        if (!behaviour.editRestPositions)
            return;
        List<Vector3> wheelRestPositions = behaviour.wheelRestPositions;

        if (wheelRestPositions.Count != 4)
            return;
        
        EditorGUI.BeginChangeCheck();
        for (int i = 0; i < 4; i++)
        {
            Vector3 newTargetPosition = Handles.PositionHandle(wheelRestPositions[i] + behaviour.transform.position, Quaternion.identity);
            wheelRestPositions[i] = newTargetPosition - behaviour.transform.position;
        }
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(behaviour, "Move Wheel Rest Position");
            behaviour.wheelRestPositions = wheelRestPositions;
            EditorUtility.SetDirty(behaviour);
        }
    }
}
#endif

[RequireComponent(typeof(Rigidbody))]
public class CarBasedDrivingBehaviour : MonoBehaviour
{
    private enum WheelID
    {
        FL,
        FR,
        RL,
        RR
    }
    [SerializeField] private LayerMask groundLayer;
    [SerializeField][Tooltip("List of all wheel transforms in the order: FL, FR, RL, RR")] private List<Transform> wheelTransforms = new List<Transform>();
    [SerializeField] private float wheelRadius = 0.25f;
    [SerializeField][Range(-1f, 1f)] private float suspensionRestLength = 0f;
    [SerializeField][Range(0f, 1f)] private float suspensionCompressionLength = 0.2f;
    [SerializeField][Range(0f, 1f)] private float suspensionExtensionLength = 0.1f;
    [SerializeField] private float suspensionStrength = 2000f;
    [SerializeField] private float suspensionDamping = 150f;
    public List<Vector3> wheelRestPositions = new List<Vector3>(new Vector3[4]);
    private bool[] wheelGrounded = new bool[4];
    [Header("Debug")]
    [SerializeField] private bool showGizmos = true;
    public bool editRestPositions = false;
    private Vector3[] rayHit = new Vector3[4];
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    } 
    void FixedUpdate()
    {
        for (int i = 0; i < wheelTransforms.Count; i++)
        {
            GravitateWheel(i);
        }
        for (int i = 0; i < wheelTransforms.Count; i++)
        {
            float force = SuspensionForce(i);
            rb.AddForceAtPosition(transform.up * force, wheelTransforms[i].position, ForceMode.Force);
        }
        // for (int i = 0; i < wheels.Count; i++)
        // {
        //     previousSuspensionOffsets[i] = suspensionRestLength - wheels[i].localPosition.y;
        // }
    }
    private void GravitateWheel(int wheel)
    {
        Ray ray = new Ray(transform.TransformPoint(wheelRestPositions[wheel]) + suspensionCompressionLength * transform.up, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, suspensionCompressionLength + suspensionExtensionLength, groundLayer))
        {
            rayHit[wheel] = hit.point;
            wheelTransforms[wheel].position = transform.TransformPoint(wheelRestPositions[wheel]) + (suspensionCompressionLength + wheelRadius - hit.distance) * transform.up;
            wheelGrounded[wheel] = true;
        }
        else
        {
            wheelGrounded[wheel] = false;
        }
    }
    private float SuspensionForce(int wheel)
    {
        if (!wheelGrounded[wheel]) return 0f;

        Vector3 wheelWorldVel = rb.GetPointVelocity(wheelTransforms[wheel].position);
        float offset = suspensionRestLength - wheelTransforms[wheel].localPosition.y;
        float strength = suspensionStrength;
        float velocity = Vector3.Dot(transform.up, wheelWorldVel);
        float damping = suspensionDamping;

        return (offset * strength) - (velocity * damping);
    }

    void OnDrawGizmos()
    {
        if (!showGizmos) return;

        Gizmos.color = Color.black;
        for(int i = 0; i < wheelRestPositions.Count; i++)
        {
            Gizmos.DrawSphere(transform.TransformPoint(wheelRestPositions[i]), 0.05f);
        }
        Gizmos.color = Color.magenta;
        for(int i = 0; i < wheelRestPositions.Count; i++)
        {
            Vector3 position = transform.TransformPoint(wheelRestPositions[i]) + suspensionCompressionLength * transform.up;
            Gizmos.DrawLine(position, position - (suspensionCompressionLength + suspensionExtensionLength) * transform.up);
        }
        Gizmos.color = Color.red;
        for (int i = 0; i < 4; i++)
        {
            Gizmos.DrawSphere(rayHit[i], 0.05f);
        }
    }
}