using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    [Header("Character Info")]
    public string characterName;
    [TextArea] 
    public string characterDescription;

    public UIButton SwapUp;
    public UIButton SwapDown;
    public UIButton SwapLeft;
    public UIButton SwapRight;

    public void Click()
    {
        GetComponent<Button>().onClick?.Invoke();
    }

    public UIButton SwapUpSelection() => SwapUp;
    public UIButton SwapDownSelection() => SwapDown;
    public UIButton SwapRightSelection() => SwapRight;
    public UIButton SwapLeftSelection() => SwapLeft;

    public GameObject characterBackground;
    public GameObject characterText;
    public void SetHighlight(bool state)
    {
        if (characterBackground)
            characterBackground.SetActive(state);
        if (characterText)
            characterText.SetActive(state);
    }
}
