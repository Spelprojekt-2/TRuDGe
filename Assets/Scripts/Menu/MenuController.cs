using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("MainMenu")]
    [SerializeField] private GameObject firstJoinPopup;
    [SerializeField] private UIButton[] mainMenuButtons;
    [SerializeField] private GameObject singlePlayerButtons;
    [SerializeField] private GameObject multiplayerButtons;
    [SerializeField] private GameObject settingsMenu;

    [Header("SelectionScreen")]
    public TextMeshProUGUI ReadyTextP1;
    public TextMeshProUGUI ReadyTextP2;
    public TextMeshProUGUI ReadyTextP3;
    public TextMeshProUGUI ReadyTextP4;
    [SerializeField] private UIButton[] preselectPlayer;

    [Header("TrackSelect/AfterRace")]
    [SerializeField] public UIButton initialSelection;
    private void Awake()
    {
        SceneController.instance.SceneChangeEvent += OnSceneChange;
    }

    void OnSceneChange()
    {
        switch (SceneController.instance.currentSceneType)
        {
            case SceneController.SceneType.MainMenu:
                MainMenuWaitForUIPopulation();
                ShowJoinPopup(PlayerTrackerManager.instance.GetPlayerCount() < 1);
                break;
            case SceneController.SceneType.PostRaceLeaderboard:
                CoroutineRunner.Run(SelectObject(initialSelection));
                break;
            case SceneController.SceneType.TrackSelectRace:
            case SceneController.SceneType.TrackSelectTimeTrial:
                CoroutineRunner.Run(SelectObject(initialSelection));
                break;
            case SceneController.SceneType.PlayerSelectRace:
                //MainMenuWaitForUIPopulation();
                CoroutineRunner.Run(SelectObject(initialSelection));
                break;
        }
    }

    public void ShowJoinPopup(bool show)
    {
        if (firstJoinPopup != null) firstJoinPopup.SetActive(show);
        if (!show)
        {
            CoroutineRunner.Run(MainMenuWaitForUIPopulation());
        }
    }
    public void ShowSinglePlayerMenu()
    {
        singlePlayerButtons.SetActive(true);
        for (int i = 0; i < mainMenuButtons.Length; i++)
        {
            mainMenuButtons[i].enabled = false;
        }
        CoroutineRunner.Run(SelectObject(singlePlayerButtons.GetComponentInChildren<UIButton>()));
    }
    public void ShowMultiplayerMenu()
    {
        multiplayerButtons.SetActive(true);
        for (int i = 0; i < mainMenuButtons.Length; i++)
        {
            mainMenuButtons[i].enabled = false;
        }
        CoroutineRunner.Run(SelectObject(multiplayerButtons.GetComponentInChildren<UIButton>()));
    }

    public void ShowSettingsMenu()
    {
        settingsMenu.SetActive(true);
        for (int i = 0; i < mainMenuButtons.Length; i++)
        {
            mainMenuButtons[i].enabled = false;
        }
        CoroutineRunner.Run(SelectObject(settingsMenu.GetComponentInChildren<UIButton>()));
    }

    public void SelectMainMenu(int buttonToSelect)
    {
        singlePlayerButtons.SetActive(false);
        multiplayerButtons.SetActive(false);
        settingsMenu.SetActive(false);
        for (int i = 0; i < mainMenuButtons.Length; i++)
        {
            mainMenuButtons[i].enabled = true;
        }
        CoroutineRunner.Run(SelectObject(mainMenuButtons[buttonToSelect]));
    }

    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void Unready()
    {
        if (RacingInformation.instance.isTimeTrial) SceneManager.LoadScene("TimeTrialMenu");
        else SceneManager.LoadScene("SelectionScreen");
    }

    public void LoadTrack(string sceneName)
    {
        if (RacingInformation.instance.isTimeTrial) SceneManager.LoadScene(sceneName);
        else
        {
            RacingInformation.instance.trackToPlay = sceneName;
            SceneManager.LoadScene("TrainingGround");
        }
    }

    public UIButton GetStartButton(int index)
    {
        return preselectPlayer[index];
    }

    private IEnumerator SelectObject(UIButton button)
    {
        yield return null;
        if (UISelection.playerSelections.Count > 0) UISelection.playerSelections[0].SwapSelection(button);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    private IEnumerator MainMenuWaitForUIPopulation()
    {
        while (mainMenuButtons == null
               || mainMenuButtons.Length == 0
               || mainMenuButtons[0] == null
               || UISelection.playerSelections.Count == 0
               || UISelection.playerSelections[0] == null)
        {
            yield return null;
        }

        UISelection.playerSelections[0].SwapSelection(mainMenuButtons[0]);
    }
}
