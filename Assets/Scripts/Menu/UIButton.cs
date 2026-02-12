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

    public void SetHighlight(bool state)
    {
        RacerData rd = FindObjectOfType<RacerData>();
        if (rd.index == 0)
        {
            if (characterBackground)
            characterBackground.SetActive(state);
            if (characterText)
            characterText.SetActive(state);
        }
        if (rd.index == 1)
        {
            if (characterBackground1)
            characterBackground1.SetActive(state);
            if (characterText1)
            characterText1.SetActive(state);
        }
        if (rd.index == 2)
        {
            if (characterBackground2)
            characterBackground3.SetActive(state);
            if (characterText2)
            characterText2.SetActive(state);
        }
        if (rd.index == 3)
        {
            if (characterBackground3)
            characterBackground3.SetActive(state);
            if (characterText3)
            characterText3.SetActive(state);
        }
    }
}
