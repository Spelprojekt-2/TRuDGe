using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class PlayerCamera : MonoBehaviour
{
    [Header("---References(REQUIRED)---")]
    [SerializeField] Transform cameraHolder;
    [SerializeField] RectTransform crosshair;
    [SerializeField] private Transform player;
    [SerializeField] private PlayerInput input;
    [SerializeField] private Transform rotationRoot;
    private AutoAimCone autoAim;

    [Header("---Camera Settings---")]
    [Tooltip("If the value is higher, the camera rotates further when mouse is near edge of screen")]
    [Range(1, 10)]
    [SerializeField] private float rotationIntensity;

    [Header("---Aiming Settings---")]
    [SerializeField] float sensitivity;
    [SerializeField] float controllerSensMultiplier;
    [SerializeField] float bottomCrosshairLimit;

    [Header("---Auto Aim Settings---")]
    public  Transform forOthersAimPoint;
    public Transform CurrentTarget { get; private set; } = null;
    public LayerMask smokeLayer;

    [Tooltip("If the value is max, the camera will move if the crosshair is moved even slightly, if the value decreases the camera will be clamped to look forward until the crosshair enters a certain distance close to the edge.")]
    [SerializeField] Vector2Int distanceFromScreenEdge;


    public Camera cam;
    private Vector2 cursorPos;
    private Vector2 screenSize;
    private Vector2 panningDist;
    private Vector2 lookInputVector;
    public bool isPressingLookBack;
    private bool isPressingResetCrosshair;
    private bool isController = false;
    private Quaternion camStartRotOffset;
    private bool lookingAtTarget = false;
    private Transform oldTarget;

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
        autoAim = GetComponentInChildren<AutoAimCone>();
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

        CurrentTarget = autoAim.GetTarget();
        if (CurrentTarget != null && IsTargetBlockedBySmoke(CurrentTarget))
        {
            CurrentTarget = null;
        }

        if (CurrentTarget != oldTarget)
        {
            crosshair.GetComponent<Image>().enabled = true;

            if (CurrentTarget != null)
            {
                StartCoroutine(FocusOnTarget());
                lookingAtTarget = true;
            }
            else
            {
                lookingAtTarget = false;
                cursorPos = new Vector2(0, 200f);
                if (TryGetComponent<PlayerShooting>(out var ps)) ps.speedMultiplier = 1f;
            }

            oldTarget = CurrentTarget;
        }
        else if (CurrentTarget != null)
        {
            crosshair.GetComponent<Image>().enabled = true;

            if (!lookingAtTarget)
            {
                StartCoroutine(FocusOnTarget());
                lookingAtTarget = true;
            }

            Vector3 screenPos = cam.WorldToScreenPoint(CurrentTarget.position);

            if (screenPos.z > 0)
            {
                float ratioX = ((screenPos.x - cam.pixelRect.xMin) / cam.pixelWidth) - 0.5f;
                float ratioY = ((screenPos.y - cam.pixelRect.yMin) / cam.pixelHeight) - 0.5f;

                RectTransform parentRect = (RectTransform)crosshair.parent;

                Vector2 localTarget;
                localTarget.x = ratioX * parentRect.rect.width;
                localTarget.y = ratioY * parentRect.rect.height;

                cursorPos = localTarget;
            }
        }
        else
        {
            StopCoroutine(FocusOnTarget());
            lookingAtTarget = false;
            cursorPos = new Vector2(0, 200f);
            crosshair.GetComponent<Image>().enabled = false;
        }

        if (crosshair != null)
        {
            crosshair.anchoredPosition = cursorPos;
        }
    }

    IEnumerator FocusOnTarget()
    {
        crosshair.localScale = new Vector3(1f, 1f, 1f);
        for (int i = 0; i < 10; i++)
        {
            crosshair.localScale -= new Vector3(0.05f, 0.05f, 0.05f);
            GetComponent<PlayerShooting>().speedMultiplier = crosshair.localScale.x;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public Ray GetStableCrosshairRay()
    {
        //Ray för denna player's kamera view
        Rect rect = cam.pixelRect;
        Vector2 center = new Vector2(rect.x + rect.width / 2f, rect.y + rect.height / 2f);
        Vector2 screenPoint = center + cursorPos;
        return cam.ScreenPointToRay(screenPoint);
    }
    public bool IsTargetBlockedBySmoke(Transform target)
    {
        Vector3 origin = cam.transform.position;
        Vector3 dir = (target.position - origin).normalized;
        float distance = Vector3.Distance(origin, target.position);

        RaycastHit[] hits = Physics.SphereCastAll(origin, 0.5f, dir, distance, ~0, QueryTriggerInteraction.Collide);

        foreach (var hit in hits)
        {
            if (((1 << hit.collider.gameObject.layer) & smokeLayer) != 0)
            {
                return true;
            }
        }
        return false;
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
