using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEditor;

public class PlayerCamera : MonoBehaviour
{
    [Header("---References(REQUIRED)---")]
    [SerializeField] Transform cameraHolder;
    [SerializeField] RectTransform crosshair;
    [SerializeField] Image previewNoRotationZone;
    [SerializeField] bool renderPreviewZone = true;
    [SerializeField] private Transform player;
    [SerializeField] private PlayerInput input;

    [Header("---Camera Settings---")]
    [Tooltip("If the value is higher, the camera rotates further when mouse is near edge of screen")]
    [Range(1, 10)]
    [SerializeField] private float rotationIntensity;

    [Tooltip("The crosshair sensitivity")]
    [SerializeField] float sensitivity;
    [SerializeField] float controllerSensMultiplier;

    [Tooltip("If the value is max, the camera will move if the crosshair is moved even slightly, if the value decreases the camera will be clamped to look forward until the crosshair enters a certain distance close to the edge.")]
    [SerializeField] Vector2Int distanceFromScreenEdge;

    private void OnValidate()
    {
        if (EditorApplication.isCompiling || EditorApplication.isUpdating)
            return;

        distanceFromScreenEdge.x = Mathf.Clamp(distanceFromScreenEdge.x, 0, 960);
        distanceFromScreenEdge.y = Mathf.Clamp(distanceFromScreenEdge.y, 0, 540);
        if(previewNoRotationZone == null)
        {
            return;
        }
        previewNoRotationZone.gameObject.SetActive(renderPreviewZone);
        if (renderPreviewZone)
        {
            previewNoRotationZone.rectTransform.sizeDelta = 2 * distanceFromScreenEdge;
        }
        
    }

    public Camera cam;
    private Vector2 cursorPos;
    private Vector2 screenSize;
    private Vector2 panningDist;
    private Vector2 lookInputVector;
    private bool isPressingLookBack, isPressingResetCrosshair;
    private Vector3 camParentOffsetPos;
    private Quaternion camParentOffsetRot;
    private bool isController = false;
    private Quaternion camStartRotOffset;

    public void LookInput(InputAction.CallbackContext context)
    {
        lookInputVector = context.ReadValue<Vector2>();
    }


    public void LookBack(InputAction.CallbackContext context)
    {
        isPressingLookBack = context.started;
    }

    public void ResetCrosshair(InputAction.CallbackContext context)
    {
        isPressingResetCrosshair = context.performed;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        camStartRotOffset = cam.transform.localRotation;
        camParentOffsetPos = cameraHolder.transform.localPosition;
        camParentOffsetRot = cameraHolder.transform.localRotation;
        isController = input.currentControlScheme == "Gamepad";
    }

    private void Update()
    {
        if (isPressingResetCrosshair)
        {
            cursorPos = Vector2.zero;
        }
    }

    void LateUpdate()
    {
        screenSize = cam.rect.size * new Vector2(Screen.width, Screen.height);


        if (isPressingLookBack)
        {
            Debug.Log("Here");
            ChangeDirection(180f);
        }
        else
        {
            ChangeDirection(0f);
        }

        cameraHolder.transform.localRotation = player.localRotation;


        Vector2 mouseDelta = lookInputVector;
        if (isController) mouseDelta *= controllerSensMultiplier;
        cursorPos += mouseDelta * sensitivity;

        cursorPos.x = Mathf.Clamp(cursorPos.x, -screenSize.x / 2, screenSize.x / 2);
        cursorPos.y = Mathf.Clamp(cursorPos.y, -screenSize.y / 2, screenSize.y / 2);
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
        cam.transform.localRotation = Quaternion.Euler(camStartRotOffset.eulerAngles.x - panningDist.y, camStartRotOffset.eulerAngles.y + panningDist.x, 0);
    }


    private void ChangeDirection(float angle)
    {
        cameraHolder.localRotation = Quaternion.Euler(0, angle + 90 + player.rotation.eulerAngles.y, 0);
    }
}
