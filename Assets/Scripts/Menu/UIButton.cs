using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIButton : MonoBehaviour
{
    public UIButton SwapUp;
    public UIButton SwapDown;
    public UIButton SwapLeft;
    public UIButton SwapRight;

    public GameObject characterDescription;
    private CharacterStats characterStats;

    [Header("P1")]
    public GameObject characterBackground;
    public GameObject characterText;
    public GameObject Stats;

    [Header("P2")]
    public GameObject characterBackground1;
    public GameObject characterText1;
    public GameObject Stats1;

    [Header("P3")]
    public GameObject characterBackground2;
    public GameObject characterText2;
    public GameObject Stats2;

    [Header("P4")]
    public GameObject characterBackground3;
    public GameObject characterText3;
    public GameObject Stats3;

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
    characterStats = GetComponent<CharacterStats>();
    characterStats.SwapCharacterStats(playerIndex);

        if (playerIndex == 0)
    {
        characterBackground?.SetActive(state);
        characterText?.SetActive(state);
        Stats?.SetActive(state);
    }
    else if (playerIndex == 1)
    {
        characterBackground1?.SetActive(state);
        characterText1?.SetActive(state);
        Stats1?.SetActive(state);
    }
    else if (playerIndex == 2)
    {
        characterBackground2?.SetActive(state);
        characterText2?.SetActive(state);
        Stats2?.SetActive(state);
    }
    else if (playerIndex == 3)
    {
        characterBackground3?.SetActive(state);
        characterText3?.SetActive(state);
        Stats3?.SetActive(state);
    }
    }
}
