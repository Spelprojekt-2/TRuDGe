using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Rendering.Universal;

public class PlayerCamera : MonoBehaviour
{
    [Header("---References(REQUIRED)---")]
    [SerializeField] Transform cameraHolder;
    [SerializeField] RectTransform crosshair;
    [SerializeField] private Transform player;
    [SerializeField] private PlayerInput input;
    [SerializeField] private Transform rotationRoot;
    [SerializeField] private AutoAimCone autoAim;

    [Header("---Camera Settings---")]
    [Tooltip("If the value is higher, the camera rotates further when mouse is near edge of screen")]
    [Range(1, 10)]
    [SerializeField] private float rotationIntensity;

    [Header("---Aiming Settings---")]
    [SerializeField] float sensitivity;
    [SerializeField] float controllerSensMultiplier;
    [SerializeField] float bottomCrosshairLimit;

    [Header("---Aim Assist Settings---")]
    [SerializeField] float aimAssistDistance;
    [SerializeField] float assistStrength = 0.2f; // How hard the crosshair pulls
    [SerializeField] float sensitivityReduction = 0.5f; // 0.5 = half speed when over enemy
    [SerializeField] LayerMask enemyLayer;

    [HideInInspector] public bool isOverEnemy = false;
    private Transform currentTarget = null;

    [Tooltip("If the value is max, the camera will move if the crosshair is moved even slightly, if the value decreases the camera will be clamped to look forward until the crosshair enters a certain distance close to the edge.")]
    [SerializeField] Vector2Int distanceFromScreenEdge;
    [Header("Debug")]
    [SerializeField] private bool showAimRay = false;


    public Camera cam;
    private Vector2 cursorPos;
    private Vector2 screenSize;
    private Vector2 panningDist;
    private Vector2 lookInputVector;
    private bool isPressingLookBack, isPressingResetCrosshair;
    private bool isController = false;
    private Quaternion camStartRotOffset;


    [SerializeField] private string uiCameraTag = "UICamera";

    public void LookInput(InputAction.CallbackContext context)
    {
        lookInputVector = context.ReadValue<Vector2>();
    }


    public void LookBack(InputAction.CallbackContext context)
    {
        isPressingLookBack = context.performed;
    }

    public void ResetCrosshair(InputAction.CallbackContext context)
    {
        isPressingResetCrosshair = context.performed;
    }

    void Start()
    {
        camStartRotOffset = cam.transform.localRotation;
        isController = input.currentControlScheme == "Gamepad";
    }


    void LateUpdate()
    {
        panningDist *= 0.01f * rotationIntensity;

        cameraHolder.transform.position = Vector3.Lerp(cameraHolder.transform.position, rotationRoot.transform.position, 10f * Time.deltaTime);
        cameraHolder.transform.rotation = Quaternion.Slerp(cameraHolder.transform.rotation, rotationRoot.transform.rotation, 10f * Time.deltaTime);
        if (isPressingLookBack)
        {
            cameraHolder.transform.localRotation = Quaternion.Euler(camStartRotOffset.eulerAngles.x - panningDist.y, 180 + camStartRotOffset.eulerAngles.y + panningDist.x, 0);
        }
        else
        {
            cameraHolder.transform.localRotation = Quaternion.Euler(camStartRotOffset.eulerAngles.x - panningDist.y, camStartRotOffset.eulerAngles.y + panningDist.x, 0);
        }

        currentTarget = autoAim.GetTarget();
        if (currentTarget != null)
        {
            Vector3 screenPoint = cam.WorldToScreenPoint(currentTarget.position);

            if (screenPoint.z > 0)
            {
                // Use cam.pixelWidth/Height for better accuracy in split-screen/scaled UI
                Vector2 centeredPos = new Vector2(
                    screenPoint.x - (cam.pixelWidth / 2f),
                    screenPoint.y - (cam.pixelHeight / 2f)
                );

                cursorPos = centeredPos;
            }
        }
        else
        {
            // This SHOULD snap the crosshair slightly above center
            cursorPos = new Vector2(0f, 80f);
        }

        // APPLY POSITION
        if (crosshair != null)
        {
            crosshair.anchoredPosition = cursorPos;
            Debug.Log($"Updating Crosshair to: {cursorPos}"); // Uncomment this to verify it's running
        }
    }

    public Ray GetStableCrosshairRay()
    {
        //pixelRect för denna player's kamera view
        Rect rect = cam.pixelRect;
        Vector2 center = new Vector2(rect.x + rect.width / 2f, rect.y + rect.height / 2f);
        Vector2 screenPoint = center + cursorPos;
        return cam.ScreenPointToRay(screenPoint);
    }

    public void MinimapPrep()
    {

        var uiCamObj = GameObject.FindWithTag(uiCameraTag);

        if (cam != null && uiCamObj != null)
        {
            Camera uiCam = uiCamObj.GetComponent<Camera>();
            var cameraData = cam.GetUniversalAdditionalCameraData();

            if (!cameraData.cameraStack.Contains(uiCam))
            {
                cameraData.cameraStack.Add(uiCam);
            }
        }
        else
        {
            Debug.LogWarning("En kamera �r null");
        }
    }
}
