using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuUI : MonoBehaviour
{
    public GameObject pauseMenu;
    public UIButton selectOnPause;

    private bool isPaused = false;
    private PlayerInput currentPlayerInput;
    private int currentPlayerID;
    private RaceController raceController;

    private void Start()
    {
        pauseMenu.SetActive(false);
        raceController = FindFirstObjectByType<RaceController>();
    }

    public void PauseGame(PlayerInput input, int playerID)
    {
        if (isPaused) return;
        pauseMenu.SetActive(true);
        if (raceController) raceController.PauseRace();
        currentPlayerInput = input;
        currentPlayerID = playerID;
        CoroutineRunner.Run(SwapMap(currentPlayerInput, "UI"));
        UISelection.playerSelections[currentPlayerID].SwapSelection(selectOnPause);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void UnpauseGame()
    {
        if (!isPaused) return;

        if (raceController) raceController.ResumeRace();
        pauseMenu.SetActive(false);
        CoroutineRunner.Run(SwapMap(currentPlayerInput, "Player"));
        Debug.Log(UISelection.playerSelections[currentPlayerID].selection);
        currentPlayerInput = null;
        Time.timeScale = 1f;
        isPaused = false;
    }

    public IEnumerator SwapMap(PlayerInput input, string map)
    {
        if (!input) yield break;
        yield return null;

        if (!input.enabled) input.enabled = true;
        if (!input.actions.enabled)
        {
            input.actions.Enable();
            yield return null;
        }
        var actionMap = input.actions.FindActionMap(map, true);
        if (actionMap != null)
        {
            input.SwitchCurrentActionMap(map);
        }
    }
}
