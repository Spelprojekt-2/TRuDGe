using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using TMPro;

public class UISelection : MonoBehaviour
{
    public static List<UISelection> playerSelections = new List<UISelection>();
    public UIButton selection;
    public UIButton lastSelection;
    public Color selectionColor;
    public RectTransform selectionHighlight;

    private RacerData racerData;
    private bool stickHeld = false;
    private UIButton[] buttonsOnScene;

    private bool isKBM;

    public string selectedCharacter;
    public static UISelection Instance;
    void Awake()
    {
        Instance = this;
    }
    public void Start()
    {
        playerSelections.Add(this);
        racerData = GetComponent<RacerData>();
        isKBM = GetComponent<PlayerInput>().currentControlScheme == "Keyboard&Mouse";
        SceneController.instance.SceneChangeEvent += UpdateButtons;
        UpdateButtons();
    }
    public void OnDisable()
    {
        SceneController.instance.SceneChangeEvent -= UpdateButtons;
        if (playerSelections.Contains(this))
        {
            playerSelections.Remove(this);
        }
    }
    public void LookInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        if (selection == null || selectionHighlight == null || !selectionHighlight.gameObject.activeSelf)
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


     public void Deselect(InputAction.CallbackContext context)
    {
            if (!context.performed) return;
            if (SceneController.instance.currentSceneType == SceneController.SceneType.PlayerSelectRace ||
            SceneController.instance.currentSceneType == SceneController.SceneType.PlayerSelectTimeTrial)
        {
            if (lastSelection == null) return;
            lastSelection.enabled = true;
            selection = null;
            SwapSelection(lastSelection);
        }
    }
    public void Select(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!selection || !selection.enabled) return;
        if (context.performed)
        {
            selectionHighlight.SetParent(transform.root.GetComponentInChildren<Canvas>().transform);
            selectionHighlight.gameObject.SetActive(false);
            selection.Click();
            lastSelection = selection;

            if (SceneManager.GetActiveScene().name == "SelectionScreen" || SceneManager.GetActiveScene().name == "TimeTrialMenu")
            {
                selection.enabled = false;
                GetComponent<SelectionScreenScript>().Ready();
            }
            else selection = null;

            if (SceneManager.GetActiveScene().name == "SelectionScreen" || SceneManager.GetActiveScene().name == "TimeTrialMenu")
            {
            string charname = selection.GetComponentInChildren<TextMeshProUGUI>().text;

            switch (charname)
            {
                case "Lars-Göran": selectedCharacter = "Lars"; break;
                case "The Brass Beast": selectedCharacter = "Nina"; break;
                case "Capôw": selectedCharacter = "Carla"; break;
                case "Schlammer": selectedCharacter = "Leonie"; break;
                case "King Napoleon III": selectedCharacter = "Napoleon"; break;
                case "Dragoș": selectedCharacter = "André"; break;
                case "Demon of Vilkmergéle": selectedCharacter = "Ragana"; break;
                case "Harlequini Martinellini": selectedCharacter = "Tristano"; break;
            }
            }
            UpdateButtons();
        }
    }
    public void MouseClicked(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (isKBM)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            RectTransform rect = selection.GetComponent<RectTransform>();
            if (!RectTransformUtility.RectangleContainsScreenPoint(rect, mousePos)) return;
        }
        Select(context);
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
        UpdateButtons();
    }

    public static void SwapPlayers(int p1index, int p2index)
    {
        if (p2index > playerSelections.Count) return;
        UISelection temp = playerSelections[p2index];
        playerSelections[p1index] = playerSelections[p2index];
        playerSelections[p1index] = temp;
    }

    public void RemovePlayer()
    {
        Destroy(selectionHighlight.gameObject);
        playerSelections.Remove(this);
        selection.RefreshUI();
        //int playerIndex = GetComponent<RacerData>().index;
        //selection.SetHighlight(false, playerIndex);
    }

    private void SelectUIUpdate(UIButton button)
    {
        if (button == null)
            return;
        
        if (selectionHighlight == null)
            return;

        int playerIndex = GetComponent<RacerData>().index;

        selectionColor = playerIndex switch
        {
            0 => Color.red,
            1 => new Color32(0,255,0,255),
            2 => new Color32(0,0,255,255),
            3 => Color.black,
        };
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

    public static void UpdateAllColors()
    {
        
    }



    private void UpdateButtons()
    {
        if (!isKBM) return;
        buttonsOnScene = FindObjectsByType<UIButton>(FindObjectsSortMode.None);
    }
    void Update()
    {
        if (!isKBM)
            return;

        if (selection == null)
            return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        if (selectionHighlight == null) return;
        Transform allowedParent = selectionHighlight.transform.parent;

        for (int i = 0; i < buttonsOnScene.Length; i++)
        {
            UIButton button = buttonsOnScene[i];
            if (button == null)
                continue;

            if (button.transform.parent != allowedParent)
                continue;

            RectTransform rect = button.GetComponent<RectTransform>();
            if (RectTransformUtility.RectangleContainsScreenPoint(rect, mousePos))
            {
                SwapSelection(button);
            }
        }
    }
}