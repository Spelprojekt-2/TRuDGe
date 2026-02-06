using UnityEngine;
using UnityEngine.InputSystem;

public class SelectionScreenScript : MonoBehaviour
{
    private PlayerTrackerManager playerTrackerManager;
    private PlayerInput input;

    private float timeSinceJoined;
    private const float joinInputDelay = 0.5f;

    private void Awake()
    {
        playerTrackerManager = FindFirstObjectByType<PlayerTrackerManager>();
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

        playerTrackerManager.SetReady(input);
    }

    public void Unready(InputAction.CallbackContext context)
    {
        if (!context.performed || !CanInteract())
            return;

        playerTrackerManager.SetUnready(input);
    }
}
