using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    public UIButton SwapUp;
    public UIButton SwapDown;
    public UIButton SwapLeft;
    public UIButton SwapRight;

    [Header("P1")]
    public GameObject characterBackground;
    public GameObject characterText;

    [Header("P2")]
    public GameObject characterBackground2;
    public GameObject characterText2;

    [Header("P3")]
    public GameObject characterBackground3;
    public GameObject characterText3;

    [Header("P4")]
    public GameObject characterBackground4;
    public GameObject characterText4;

    public void Click()
    {
        GetComponent<Button>().onClick?.Invoke();
    }

    public UIButton SwapUpSelection() => SwapUp;
    public UIButton SwapDownSelection() => SwapDown;
    public UIButton SwapRightSelection() => SwapRight;
    public UIButton SwapLeftSelection() => SwapLeft;

    public void SetHighlight(bool state)
    {
        int playerIndex = GetComponent<RacerData>().index;
        if (playerIndex == 0)
        {
            if (characterBackground)
            characterBackground.SetActive(state);
            if (characterText)
            characterText.SetActive(state);
        }
        if (playerIndex == 1)
        {
            if (characterBackground2)
            characterBackground2.SetActive(state);
            if (characterText2)
            characterText2.SetActive(state);
        }
        if (playerIndex == 2)
        {
            if (characterBackground3)
            characterBackground3.SetActive(state);
            if (characterText3)
            characterText3.SetActive(state);
        }
        if (playerIndex == 3)
        {
            if (characterBackground4)
            characterBackground4.SetActive(state);
            if (characterText4)
            characterText4.SetActive(state);
        }
    }
}
