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
    [SerializeField] private GameObject[] characterSprites;

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
                characterSprites[playerIndex]?.SetActive(state);
            }
        }
    }
}
