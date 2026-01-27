using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform cameraHolder;
    public float mouseSens = 0.2f;
    private float verticalRotation;
    private float horizontalRotation;
    [SerializeField] private Transform cameraAnchor;
    [SerializeField] private float cameraRotationSpeed = 0.5f;
    private bool rotationStarted = false;
    private Mouse mouse;
    private Camera main;

    private void Start()
    {
        mouse = Mouse.current;
        main = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        if (Keyboard.current.xKey.isPressed)
        {
            ChangeDirection(180f);
        }
        else
        {
            ChangeDirection(0f);
        }
    }
    void LateUpdate()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        horizontalRotation += mouseDelta.x * mouseSens;
        verticalRotation -= mouseDelta.y * mouseSens;

        verticalRotation = Mathf.Clamp(verticalRotation, -10f, 10f);
        horizontalRotation = Mathf.Clamp(horizontalRotation, -110f, -80f);

        main.transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
    }

    private void ChangeDirection(float angle)
    {
        cameraHolder.localRotation = Quaternion.Euler(0, angle, 0);
    }

    private IEnumerator ChangeDirectionRoutine(float angle)
    {
        rotationStarted = true;

        float timePassed = 0f;
        Quaternion startRotation = cameraAnchor.localRotation;
        Quaternion endRotation = Quaternion.Euler(0, angle, 0);

        while (timePassed < cameraRotationSpeed)
        {
            timePassed += Time.deltaTime;
            float t = timePassed / cameraRotationSpeed;

            float smoothT = t * t * (3f - 2f * t);

            cameraAnchor.localRotation = Quaternion.Slerp(startRotation, endRotation, smoothT);
            yield return null;
        }

        cameraAnchor.localRotation = endRotation;
        rotationStarted = false;
    }
}
