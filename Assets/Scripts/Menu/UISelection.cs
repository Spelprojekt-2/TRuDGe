using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using TMPro;

public class UISelection : MonoBehaviour
{
    public static List<UISelection> playerSelections = new List<UISelection>();
    public UIButton selection;
    public Color selectionColor;
    public RectTransform selectionHighlight;
    public GameObject characterBackground;
    public GameObject characterText;

    public void Start()
    {
        playerSelections.Add(this);
    }
    public void OnDestroy()
    {
        if (playerSelections.Contains(this))
        {
            playerSelections.Remove(this);
        }
    }
    public void LookInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        int playerIndex = GetComponent<RacerData>().index;
        if (input == Vector2.zero || !selection || !selectionHighlight.gameObject.activeSelf)
            return;

        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            if (input.x > 0) SwapSelection(selection.SwapRightSelection());
            else SwapSelection(selection.SwapLeftSelection());
        }
        else
        {
            if (input.y > 0) SwapSelection(selection.SwapUpSelection());
            else SwapSelection(selection.SwapDownSelection());
        }
    }

    public void Clicked(InputAction.CallbackContext context)
    {
        if (!selection) return;
        if (context.performed)
        {
            selectionHighlight.parent = transform.root.GetComponentInChildren<Canvas>().transform;
            selectionHighlight.gameObject.SetActive(false);
            selection.Click();
        }
    }

    public void SwapSelection(UIButton newButton)
{
    if (!newButton || selection == newButton)
        return;

    if (selection)
        selection.SetHighlight(false);

    selection = newButton;

    selection.SetHighlight(true);

    SelectUIUpdate(newButton);
    UpdateCharacterText(newButton);
}

    public void SwapPlayers(int p1index, int p2index)
    {
        UISelection temp = playerSelections[p2index];
        playerSelections[p1index] = playerSelections[p2index];
        playerSelections[p2index] = temp;
    }

    public static void RemovePlayer(UISelection selection)
    {
        if (playerSelections.Contains(selection))
        {
            playerSelections.Remove(selection);
        }
    }

    private void SelectUIUpdate(UIButton button)
    {
        selectionHighlight.gameObject.SetActive(true);
        selectionHighlight.transform.parent = button.transform.parent;
        selectionHighlight.SetSiblingIndex(0);
        selectionHighlight.position = button.GetComponent<RectTransform>().position;
        selectionHighlight.localScale = Vector3.one;
        selectionHighlight.rotation = Quaternion.identity;
        selectionHighlight.sizeDelta = button.GetComponent<RectTransform>().sizeDelta + new Vector2(30,30);

        characterBackground.gameObject.SetActive(true); characterText.gameObject.SetActive(true);
    }
    private void UpdateCharacterText(UIButton button)
    {
        TMPro.TMP_Text textComponent = characterText.GetComponent<TMPro.TMP_Text>();

        if (textComponent != null)
        {
            textComponent.text = button.characterName + "\n\n" + button.characterDescription;
        }
    }
}