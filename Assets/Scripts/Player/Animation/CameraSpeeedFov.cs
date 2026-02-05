using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraSpeeedFov : MonoBehaviour
{
    private const float X_AXIS_SCALE = 100f;
    private const float Y_AXIS_SCALE = 10f;
    private Camera cam;
    [SerializeField] private PlayerMovement playerMovement;
    [Tooltip("Fov based on speed\n0 < Speed < 100\nFov = curve * 10")]
    [SerializeField] private AnimationCurve fovOverSpeed = AnimationCurve.Linear(0, 6, 1, 6);
    [Tooltip("How fast the fov adjusts to speed changes")]
    [SerializeField] private float fovAdjustmentSpeed = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        float speed = playerMovement.GetCurrentSpeed(true);
        float maxSpeed = playerMovement.GetTopSpeed();
        speed = Mathf.Clamp01(speed / maxSpeed) * X_AXIS_SCALE;
        float targetFov = fovOverSpeed.Evaluate(speed) * Y_AXIS_SCALE;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFov, Time.deltaTime * fovAdjustmentSpeed);
    }
}
