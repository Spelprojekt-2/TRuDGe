using UnityEngine;
using UnityEngine.InputSystem;

public class SelectionScreenScript : MonoBehaviour
{
    private PlayerInput input;

    private float timeSinceJoined;
    private const float joinInputDelay = 0.5f;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        timeSinceJoined = Time.realtimeSinceStartup;
    }

    private bool CanInteract()
    {
        return Time.realtimeSinceStartup - timeSinceJoined >= joinInputDelay;
    }

    public void Ready(InputAction.CallbackContext context)
    {
        if (!context.performed || !CanInteract())
            return;

        PlayerTrackerManager.instance.SetReady(input);
    }

    public void Unready(InputAction.CallbackContext context)
    {
        if (!context.performed || !CanInteract())
            return;

        PlayerTrackerManager.instance.SetUnready(input);
    }

    public void Disconnect(InputAction.CallbackContext context)
    {
        if (!context.performed || !CanInteract())
            return;

        PlayerTrackerManager.instance.HandlePlayerLeft(input);
    }
}
