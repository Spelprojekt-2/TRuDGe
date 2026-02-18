using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPause : MonoBehaviour
{
    private PauseMenuUI pauseMenuUI;

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            pauseMenuUI = FindFirstObjectByType<PauseMenuUI>();
            if (pauseMenuUI) pauseMenuUI.PauseGame(GetComponent<PlayerInput>(), GetComponent<RacerData>().index);
        }
    }
}
