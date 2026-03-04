using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class UIButton : MonoBehaviour
{
    private Button button;
    public UIButton SwapUp;
    public UIButton SwapDown;
    public UIButton SwapLeft;
    public UIButton SwapRight;

    public GameObject characterDescription;
    private CharacterStats Stats;

    [Header ("Character Info")]
    [SerializeField] private GameObject[] characterBackgrounds;
    [SerializeField] private GameObject[] characterText;
    [SerializeField] private GameObject[] characterStats;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Click()
    {
        GetComponent<Button>().onClick?.Invoke();
    }

    public UIButton SwapUpSelection() => SwapUp;
    public UIButton SwapDownSelection() => SwapDown;
    public UIButton SwapRightSelection() => SwapRight;
    public UIButton SwapLeftSelection() => SwapLeft;

    public void SetHighlight(bool state, int playerIndex)
    {
        if (SceneManager.GetActiveScene().name == "SelectionScreen" || SceneManager.GetActiveScene().name == "TimeTrialMenu")
        {
            Stats = GetComponent<CharacterStats>();
            if (!Stats) return; 
            Stats.SwapCharacterStats(playerIndex);
            if (playerIndex >= 0 && playerIndex < characterBackgrounds.Length)
            {
                characterBackgrounds[playerIndex]?.SetActive(state);
                characterText[playerIndex]?.SetActive(state);
                characterStats[playerIndex]?.SetActive(state);
            }
        }
    }
    //Disable highest in array??
    public void RefreshUI()
    {
        int activePlayers = PlayerTrackerManager.instance.GetPlayerCount();
        characterBackgrounds[activePlayers].SetActive(false);
        characterText[activePlayers].SetActive(false);
        characterStats[activePlayers].SetActive(false);
        /*for (int i = 0; i < characterBackgrounds.Length; i++)
    {
        bool shouldBeActive = i < activePlayers;

            characterBackgrounds[i]?.SetActive(shouldBeActive);
            characterText[i]?.SetActive(shouldBeActive);
            characterStats[i]?.SetActive(shouldBeActive);
        }
    }
        GameObject lastElement = activePlayers
        GameObject lastElement1 = characterText[characterText.Length - 1];
        GameObject lastElement2 = characterStats[characterStats.Length - 1];
        lastElement.SetActive(false);
        lastElement1.SetActive(false);
        lastElement2.SetActive(false);*/
        
    }
}
