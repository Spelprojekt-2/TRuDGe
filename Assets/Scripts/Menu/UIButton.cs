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

    [Header("P1")]
    public GameObject characterBackground;
    public GameObject characterText;

    [Header("P2")]
    public GameObject characterBackground1;
    public GameObject characterText1;

    [Header("P3")]
    public GameObject characterBackground2;
    public GameObject characterText2;

    [Header("P4")]
    public GameObject characterBackground3;
    public GameObject characterText3;

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
        if (playerIndex == 0)
    {
        characterBackground?.SetActive(state);
        characterText?.SetActive(state);
    }
    else if (playerIndex == 1)
    {
        characterBackground1?.SetActive(state);
        characterText1?.SetActive(state);
    }
    else if (playerIndex == 2)
    {
        characterBackground2?.SetActive(state);
        characterText2?.SetActive(state);
    }
    else if (playerIndex == 3)
    {
        characterBackground3?.SetActive(state);
        characterText3?.SetActive(state);
    }
    
    }
}
