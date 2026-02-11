using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [SerializeField] private GameObject firstJoinPopup;
    [SerializeField] private UIButton[] mainMenuButtons;
    [SerializeField] private GameObject singlePlayerButtons;
    [SerializeField] private GameObject multiplayerButtons;
    [SerializeField] private GameObject settingsMenu;

    private void Start()
    {
        if (UISelection.playerSelections.Count > 0) UISelection.playerSelections[0].SwapSelection(mainMenuButtons[0]);
        ShowJoinPopup(PlayerTrackerManager.instance.GetPlayerCount() < 1);
    }
    public void ShowJoinPopup(bool show)
    {
        firstJoinPopup.SetActive(show);
        if (!show)
        {
            CoroutineRunner.Run(SelectObject(mainMenuButtons[0]));
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

    private IEnumerator SelectObject(UIButton button)
    {
        yield return null;
        if (UISelection.playerSelections.Count > 0) UISelection.playerSelections[0].SwapSelection(button);
    }
}
