using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{

    public GameObject pauseMenu;
    public GameObject mainPauseMenu;
    public GameObject settingsMenu;
    public GameObject controlsMenu;
    public GameObject audioMenu;
    public UIButton selectOnPause;
    public UIButton selectOnSettings;
    public UIButton selectOnControls;
    public UIButton selectOnAudio;
    private bool isPaused = false;
    private PlayerInput currentPlayerInput;
    private int currentPlayerID;
    private RaceController raceController;
    public string scen;

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
        Cursor.lockState = CursorLockMode.None;
        UISelection.playerSelections[currentPlayerID].SwapSelection(selectOnPause);
        Time.timeScale = 0f;
        isPaused = true;

        // Pause audio
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.TogglePause(true);
        }
    }

    public void ExitRace()
    {
        if (RacingInformation.instance.isTimeTrial) SceneManager.LoadScene("TrackSelectTimeTrial");
        else SceneManager.LoadScene("TrackSelect");
    }

    public void UnpauseGame()
    {
        if (!isPaused) return;

        if (raceController) raceController.ResumeRace();
        pauseMenu.SetActive(false);
        CoroutineRunner.Run(SwapMap(currentPlayerInput, "Player"));
        Cursor.lockState = CursorLockMode.Locked;
        currentPlayerInput = null;
        Time.timeScale = 1f;
        isPaused = false;

        // Unpause audio
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.TogglePause(false);
        }
    }

    public void OpenSettings()
    {
        settingsMenu.SetActive(true);
        CoroutineRunner.Run(SwapButton(selectOnSettings));
    }

    public IEnumerator SwapButton(UIButton btn)
    {
        yield return null;
        UISelection.playerSelections[currentPlayerID].SwapSelection(btn);
    }
    public void CloseSettings()
    {
        mainPauseMenu.SetActive(true);
        settingsMenu.SetActive(false);
        CoroutineRunner.Run(SwapButton(selectOnPause));
    }

    public void OpenControls()
    {
        mainPauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(true);
        CoroutineRunner.Run(SwapButton(selectOnControls));
    }

    public void CloseControls()
    {
        controlsMenu.SetActive(false);
        settingsMenu.SetActive(true);
        CoroutineRunner.Run(SwapButton(selectOnSettings));
    }

    public void OpenAudio()
    {
        settingsMenu.SetActive(false);
        audioMenu.SetActive(true);
        CoroutineRunner.Run(SwapButton(selectOnAudio));
    }

    public void CloseAudio()
    {
        settingsMenu.SetActive(true);
        audioMenu.SetActive(false);
        CoroutineRunner.Run(SwapButton(selectOnSettings));
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
    public void SwapScene()
    {
        SceneManager.LoadScene(scen);
    }
}
