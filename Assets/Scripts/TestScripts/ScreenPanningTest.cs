using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ScreenPanningTest : MonoBehaviour
{
    Camera cam;
    [SerializeField] float sensitivity;
    [SerializeField] Vector2 panAtDistanceFromEdge;
    [SerializeField] RectTransform crosshair;
    private Vector2 cursorPos;
    private Vector2 screenSize;
    private Vector2 panningDist;
    [SerializeField] private float panningScale;
    private Quaternion camStartRotOffset;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main;
        screenSize = new Vector2 (Screen.width, Screen.height);
        camStartRotOffset = cam.transform.localRotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        cursorPos += mouseDelta * sensitivity;
        cursorPos.x = Mathf.Clamp(cursorPos.x, -screenSize.x/2, screenSize.x/2);
        cursorPos.y = Mathf.Clamp(cursorPos.y, -screenSize.y/2, screenSize.y/2);
        crosshair.anchoredPosition = cursorPos;
        if (cursorPos.x > screenSize.x / 2 - panAtDistanceFromEdge.x) // Right
        {
            panningDist.x = cursorPos.x - (screenSize.x / 2 - panAtDistanceFromEdge.x);
        }
        else if (cursorPos.x < -screenSize.x / 2 + panAtDistanceFromEdge.x) // Left
        {
            panningDist.x = cursorPos.x - (-screenSize.x / 2 + panAtDistanceFromEdge.x);
        }
        else
        {
            panningDist.x = 0;
        }

        if (cursorPos.y > screenSize.y / 2 - panAtDistanceFromEdge.y) // Up
        {
            panningDist.y = cursorPos.y - (screenSize.y / 2 - panAtDistanceFromEdge.y);
        }
        else if (cursorPos.y < -screenSize.y / 2 + panAtDistanceFromEdge.y) // Down
        {
            panningDist.y = cursorPos.y - (-screenSize.y / 2 + panAtDistanceFromEdge.y);
        }
        else
        {
            panningDist.y = 0;
        }

        panningDist *= panningScale;
        cam.transform.localRotation = Quaternion.Euler(camStartRotOffset.eulerAngles.x - panningDist.y, camStartRotOffset.eulerAngles.y + panningDist.x, 0);
    }
}
