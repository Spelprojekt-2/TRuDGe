using UnityEngine;
using UnityEngine.InputSystem;

public class SelectionScreenScript : MonoBehaviour
{
    private PlayerInput input;
    private UIButton uibutton;

    private float timeSinceJoined;
    private const float joinInputDelay = 0.1f;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        timeSinceJoined = Time.realtimeSinceStartup;
    }

    private bool CanInteract()
    {
        return (SceneController.instance.currentSceneType == SceneController.SceneType.PlayerSelectRace ||
            SceneController.instance.currentSceneType == SceneController.SceneType.PlayerSelectTimeTrial)
            && Time.realtimeSinceStartup - timeSinceJoined >= joinInputDelay;
    }

    public void Ready()
    {
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
