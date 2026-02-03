using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEditor;

public class PlayerCamera : MonoBehaviour
{
    [Header("---References(REQUIRED)---")]
    [SerializeField] Transform cameraHolder;
    [SerializeField] RectTransform crosshair;
    [SerializeField] private Transform player;
    [SerializeField] private PlayerInput input;

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

    private bool isOverEnemy = false;

    [Tooltip("If the value is max, the camera will move if the crosshair is moved even slightly, if the value decreases the camera will be clamped to look forward until the crosshair enters a certain distance close to the edge.")]
    [SerializeField] Vector2Int distanceFromScreenEdge;


    public Camera cam;
    private Vector2 cursorPos;
    private Vector2 screenSize;
    private Vector2 panningDist;
    private Vector2 lookInputVector;
    private bool isPressingLookBack, isPressingResetCrosshair;
    private bool isController = false;
    private Quaternion camStartRotOffset;

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
        Cursor.lockState = CursorLockMode.Locked;
        camStartRotOffset = cam.transform.localRotation;
        isController = input.currentControlScheme == "Gamepad";
    }

    private void Update()
    {
        if (isPressingResetCrosshair)
        {
            cursorPos = new Vector2(0f, 100f);
        }
    }

    void LateUpdate()
    {
        screenSize = cam.rect.size * new Vector2(Screen.width, Screen.height);

        ApplyAimAssist();

        Vector2 mouseDelta = lookInputVector;
        if (isController) mouseDelta *= controllerSensMultiplier;

        float currentSens = isOverEnemy ? sensitivity * sensitivityReduction : sensitivity;
        cursorPos += mouseDelta * currentSens;

        cursorPos.x = Mathf.Clamp(cursorPos.x, -screenSize.x / 2, screenSize.x / 2);
        cursorPos.y = Mathf.Clamp(cursorPos.y, -bottomCrosshairLimit, screenSize.y / 2);
        crosshair.anchoredPosition = cursorPos;



        if (cursorPos.x > screenSize.x / 2 - distanceFromScreenEdge.x) // Right
        {
            panningDist.x = cursorPos.x - (screenSize.x / 2 - distanceFromScreenEdge.x);
        }
        else if (cursorPos.x < -screenSize.x / 2 + distanceFromScreenEdge.x) // Left
        {
            panningDist.x = cursorPos.x - (-screenSize.x / 2 + distanceFromScreenEdge.x);
        }
        else
        {
            panningDist.x = 0;
        }

        if (cursorPos.y > screenSize.y / 2 - distanceFromScreenEdge.y) // Up
        {
            panningDist.y = cursorPos.y - (screenSize.y / 2 - distanceFromScreenEdge.y);
        }
        else if (cursorPos.y < -screenSize.y / 2 + distanceFromScreenEdge.y) // Down
        {
            panningDist.y = cursorPos.y - (-screenSize.y / 2 + distanceFromScreenEdge.y);
        }
        else
        {
            panningDist.y = 0;
        }

        panningDist *= 0.01f * rotationIntensity;

        if (isPressingLookBack)
        {
            cameraHolder.transform.localRotation = Quaternion.Euler(camStartRotOffset.eulerAngles.x - panningDist.y, 180 + camStartRotOffset.eulerAngles.y + panningDist.x, 0);
        }
        else
        {
            cameraHolder.transform.localRotation = Quaternion.Euler(camStartRotOffset.eulerAngles.x - panningDist.y, camStartRotOffset.eulerAngles.y + panningDist.x, 0);
        }
        
    }

    public Ray GetStableCrosshairRay()
    {
        // Use pixelRect to get the size of THIS player's specific window
        Rect rect = cam.pixelRect;

        // Calculate the center of THIS camera's viewport
        // rect.x and rect.y handle the offset (e.g. if the camera starts halfway down the screen)
        Vector2 center = new Vector2(rect.x + rect.width / 2f, rect.y + rect.height / 2f);

        // Add your cursorPos (which is relative to the center of the player's UI)
        Vector2 screenPoint = center + cursorPos;

        // Create the ray
        return cam.ScreenPointToRay(screenPoint);
    }

    void ApplyAimAssist()
    {
        Ray ray = cam.ScreenPointToRay(crosshair.position);
        isOverEnemy = false;

        if (Physics.SphereCast(ray, aimAssistDistance, out RaycastHit hit, 100f, enemyLayer))
        {
            if (hit.collider.gameObject != player.gameObject)
            {
                isOverEnemy = true;

                if (hit.transform.root == player.root)
                {
                    return; // It hit us, so stop here and don't apply assist
                }

                Vector3 rawScreenPoint = cam.WorldToScreenPoint(hit.collider.bounds.center);
                Vector2 centeredTarget;
                centeredTarget.x = rawScreenPoint.x - (Screen.width * cam.rect.x) - (screenSize.x / 2f);
                centeredTarget.y = rawScreenPoint.y - (Screen.height * cam.rect.y) - (screenSize.y / 2f);

                if (lookInputVector.magnitude > 0.01f)
                {
                    cursorPos = Vector2.Lerp(cursorPos, centeredTarget, assistStrength * Time.deltaTime * 5f);
                }
                Debug.DrawLine(cam.transform.position, hit.point, Color.yellow);
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (cam == null || crosshair == null) return;

        Ray ray = cam.ScreenPointToRay(crosshair.position);
        float maxDist = 100f;

        Gizmos.color = isOverEnemy ? Color.green : Color.red;

        Gizmos.DrawWireSphere(transform.position, aimAssistDistance);

        Gizmos.DrawRay(transform.position, ray.direction * maxDist);

        Gizmos.DrawWireSphere(transform.position + ray.direction * maxDist, aimAssistDistance);

    }
}
