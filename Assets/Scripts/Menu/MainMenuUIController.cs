using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [SerializeField] private GameObject firstJoinPopup;
    [SerializeField] private Button[] mainMenuButtons;
    [SerializeField] private GameObject singlePlayerButtons;
    [SerializeField] private GameObject multiplayerButtons;
    [SerializeField] private GameObject settingsMenu;

    private void Start()
    {
        ShowJoinPopup(PlayerTrackerManager.instance.GetPlayerCount() < 1);
    }
    public void ShowJoinPopup(bool show)
    {
        firstJoinPopup.SetActive(show);
        if (!show) StartCoroutine(Coroutines.SelectButton(mainMenuButtons[0].gameObject));
    }
    public void ShowSinglePlayerMenu()
    {
        singlePlayerButtons.SetActive(true);
        for (int i = 0; i < mainMenuButtons.Length; i++)
        {
            mainMenuButtons[i].enabled = false;
        }
        SelectObject(singlePlayerButtons.GetComponentInChildren<Button>().gameObject);
    }
    public void ShowMultiplayerMenu()
    {
        multiplayerButtons.SetActive(true);
        for (int i = 0; i < mainMenuButtons.Length; i++)
        {
            mainMenuButtons[i].enabled = false;
        }
        SelectObject(multiplayerButtons.GetComponentInChildren<Button>().gameObject);
    }

    public void ShowSettingsMenu()
    {
        settingsMenu.SetActive(true);
        for (int i = 0; i < mainMenuButtons.Length; i++)
        {
            mainMenuButtons[i].enabled = false;
        }
        SelectObject(settingsMenu.GetComponentInChildren<Button>().gameObject);
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
        SelectObject(mainMenuButtons[buttonToSelect].gameObject);
    }

    public static void SelectObject(GameObject go)
    {
        EventSystem.current.SetSelectedGameObject(go);
    }
}
