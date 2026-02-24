using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class UISelection : MonoBehaviour
{
    public static List<UISelection> playerSelections = new List<UISelection>();
    public UIButton selection;
    public Color selectionColor;
    public RectTransform selectionHighlight;

    private RacerData racerData;
    private bool stickHeld = false;
    void Awake()
    {
        racerData = GetComponent<RacerData>();
    }
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

        if (!selection || !selectionHighlight.gameObject.activeSelf)
            return;

        if (input.magnitude < 0.5f)
        {
            stickHeld = false;
            return;
        }
        if (stickHeld)
            return;

        stickHeld = true;

        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            if (input.x > 0)
                SwapSelection(selection.SwapRightSelection());
            else
                SwapSelection(selection.SwapLeftSelection());
        }
        else
        {
            if (input.y > 0)
                SwapSelection(selection.SwapUpSelection());
            else
                SwapSelection(selection.SwapDownSelection());
        }
    }

    public void Clicked(InputAction.CallbackContext context)
    {
        if (!selection) return;
        if (context.performed)
        {
            selectionHighlight.SetParent(transform.root.GetComponentInChildren<Canvas>().transform);
            selectionHighlight.gameObject.SetActive(false);
            selection.Click();
            selection = null;
        }
    }

    public void SwapSelection(UIButton newButton)
    {
    int playerIndex = GetComponent<RacerData>().index;
    if (!newButton || selection == newButton)
        return;

    if (selection) selection.SetHighlight(false, playerIndex);

    selection = newButton;

    selection.SetHighlight(true, playerIndex);

    SelectUIUpdate(newButton);
    }

    public void SwapPlayers(int p1index, int p2index)
    {
        UISelection temp = playerSelections[p2index];
        playerSelections[p1index] = playerSelections[p2index];
        playerSelections[p1index] = temp;
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
        if (button == null)
            return;
        
        if (selectionHighlight == null)
            return;

        int playerIndex = GetComponent<RacerData>().index;
        if (playerIndex == 0)
        {
            selectionColor = Color.red;
        }
        else if (playerIndex == 1)
        {
            selectionColor = new Color32(0,255,0,255);
        }
        else if (playerIndex == 2)
        {
            selectionColor = new Color32(0,0,255,255);
        }
        else if (playerIndex == 3)
        {
            selectionColor = Color.black;
        }
        Image highlightImage = selectionHighlight.GetComponent<Image>();
        if (highlightImage == null)
            return;
        highlightImage.color = selectionColor;

        selectionHighlight.gameObject.SetActive(true);
        if (button.transform != null)   
        selectionHighlight.transform.SetParent(button.transform.parent);

        UpdateAllHighlights();
    }

    private void UpdateAllHighlights()
    {
    int maxPlayers = playerSelections.Count;

    var grouped = playerSelections
        .Where(p => p.selection != null) 
        .GroupBy(p => p.selection);

    foreach (var group in grouped)
    {
        var sorted = group
            .OrderByDescending(p => p.GetComponent<RacerData>().index)
            .ToList();

        for (int i = 0; i < sorted.Count; i++)
        {
            UISelection ui = sorted[i];

            if (ui.selection == null || ui.selectionHighlight == null)
                continue; 

            RectTransform highlight = ui.selectionHighlight;
            RectTransform buttonRect =
                ui.selection.GetComponent<RectTransform>();

            if (buttonRect == null)
                continue; 

            int clampedIndex = Mathf.Clamp(i, 0, maxPlayers - 1);

            float basePadding = 25f;
            float paddingStep = 15f;

            highlight.sizeDelta = buttonRect.sizeDelta +
            Vector2.one * (basePadding + paddingStep * clampedIndex);

            highlight.localScale = Vector3.one;
            highlight.position = buttonRect.position;
            highlight.rotation = Quaternion.identity;

            highlight.SetSiblingIndex(sorted.Count - 1 - i);
        }
    }
}
}