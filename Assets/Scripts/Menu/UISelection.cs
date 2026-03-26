using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using TMPro;
using System.Numerics;
using Vector2 = UnityEngine.Vector2;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class UISelection : MonoBehaviour
{
    [SerializeField] private TankMaterializer2000 tankMaterializer;
    public static List<UISelection> playerSelections = new List<UISelection>();
    public UIButton selection;
    public UIButton lastSelection;
    public Color selectionColor;
    public RectTransform selectionHighlight;
    private Slider draggingSlider = null;

    [SerializeField] private Image[] buttonBGs;

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
            {
                Slider slider = selection.GetComponent<Slider>();
                if (slider != null)
                {
                    slider.value += 0.01f;
                    stickHeld = false;
                }
                else SwapSelection(selection.SwapRightSelection());
            }
            else
            {
                Slider slider = selection.GetComponent<Slider>();
                if (slider != null)
                {
                    slider.value -= 0.01f;
                    stickHeld = false;
                }
                SwapSelection(selection.SwapLeftSelection());
            }
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
            if (lastSelection != selection) lastSelection = null;
            lastSelection.enabled = true;
            selection = null;
            SwapSelection(lastSelection);
            lastSelection.GetComponent<Image>().color = Color.white;
            lastSelection = null;
        }
    }

    public void Select(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!selection || !selection.enabled) return;
        if (selection.GetComponent<Slider>() != null) return;
        {
            TextMeshProUGUI textObj = selection.GetComponentInChildren<TextMeshProUGUI>();
            string charname = (textObj != null) ? textObj.text : null;

            if (SceneController.instance.currentSceneType == SceneController.SceneType.CharScreen && selection.transform.GetSiblingIndex() != selection.transform.parent.childCount - 1) return;

            selectionHighlight.SetParent(transform.root.GetComponentInChildren<Canvas>().transform);
            selectionHighlight.gameObject.SetActive(false);
            selection.Click();
            lastSelection = selection;

            if (SceneController.instance.currentSceneType == SceneController.SceneType.PlayerSelectRace || SceneController.instance.currentSceneType == SceneController.SceneType.PlayerSelectTimeTrial)
            {
                selection.enabled = false;
                GetComponent<SelectionScreenScript>().Ready();
            }
            else selection = null;

            int playerIndex = GetComponent<RacerData>().index;

            if (SceneController.instance.currentSceneType == SceneController.SceneType.PlayerSelectRace || SceneController.instance.currentSceneType == SceneController.SceneType.PlayerSelectTimeTrial)
            {
                selection.GetComponent<Image>().color = playerIndex switch
                {
                    0 => new Color32(255, 25, 25, 255),
                    1 => new Color32(50, 200, 50, 255),
                    2 => new Color32(255, 255, 0, 255),
                    3 => new Color32(0, 190, 255, 255),
                    _ => new Color32(255, 255, 255, 255)
                };
                Debug.Log(charname);
                if (charname != null)
                {
                    switch (charname)
                    {
                        case "Lars-Göran": selectedCharacter = "Lars-Göran"; break;
                        case "The Brass Beast": selectedCharacter = "The Brass Beast"; break;
                        case "Capôw": selectedCharacter = "Capôw"; break;
                        case "Schlammer": selectedCharacter = "Schlammer"; break;
                        case "King Napoleon III": selectedCharacter = "King Napoleon III"; break;
                        case "Dragoș": selectedCharacter = "Dragoș"; break;
                        case "Demon of Vilkmergéle": selectedCharacter = "Demon of Vilkmergéle"; break;
                        case "Harlequini Martinellini": selectedCharacter = "Harlequini Martinellini"; break;
                    }
                    racerData.SetName(selectedCharacter);
                }
            }
        }
        UpdateButtons();
    }

    public void MouseClicked(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (isKBM)
        {
            if (selection == null) return;
            Vector2 mousePos = Mouse.current.position.ReadValue();
            RectTransform rect = selection.GetComponent<RectTransform>();
            if (!RectTransformUtility.RectangleContainsScreenPoint(rect, mousePos)) return;
        }

        if (selection.TryGetComponent<Slider>(out Slider slider))
        {
            draggingSlider = slider;
            UpdateSliderValue(slider);
        }
        else Select(context);
    }

    public void MouseReleased(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        draggingSlider = null;
    }

    private void UpdateSliderValue(Slider slider)
    {
        RectTransform rect = slider.GetComponent<RectTransform>();
        Vector2 mousePos = Mouse.current.position.ReadValue();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rect,
            mousePos,
            null,
            out Vector2 localPoint
        );

        float normalized;

        if (slider.direction == Slider.Direction.LeftToRight ||
            slider.direction == Slider.Direction.RightToLeft)
        {
            normalized = Mathf.InverseLerp(rect.rect.xMin, rect.rect.xMax, localPoint.x);
        }
        else
        {
            normalized = Mathf.InverseLerp(rect.rect.yMin, rect.rect.yMax, localPoint.y);
        }

        if (slider.direction == Slider.Direction.RightToLeft ||
            slider.direction == Slider.Direction.TopToBottom)
        {
            normalized = 1f - normalized;
        }

        slider.value = Mathf.Lerp(slider.minValue, slider.maxValue, normalized);
    }

    public void SwapSelection(UIButton newButton)
    {
        int playerIndex = GetComponent<RacerData>().index;
        if (!newButton || selection == newButton)
        {
            return;
        }
        if (selection) selection.SetHighlight(false, playerIndex);
        selection = newButton;
        selection.SetHighlight(true, playerIndex);
        SelectUIUpdate(newButton);
        UpdateButtons();

        if (SceneController.instance.currentSceneType == SceneController.SceneType.PlayerSelectRace || SceneController.instance.currentSceneType == SceneController.SceneType.PlayerSelectTimeTrial || SceneController.instance.currentSceneType == SceneController.SceneType.CharScreen)
        {
            TextMeshProUGUI textObj = selection.GetComponentInChildren<TextMeshProUGUI>();
            string charname = (textObj != null) ? textObj.text : null;

            switch (charname)
            {
                case "Lars-Göran":
                    tankMaterializer.SwitchMaterialScheme(0);
                    break;
                case "The Brass Beast":
                    tankMaterializer.SwitchMaterialScheme(1);
                    break;
                case "Capôw":
                    tankMaterializer.SwitchMaterialScheme(2);
                    break;
                case "King Napoleon III":
                    tankMaterializer.SwitchMaterialScheme(3);
                    break;
                default:
                    tankMaterializer.SwitchMaterialScheme(4);
                    break;
            }
        }
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
    }

    private void SelectUIUpdate(UIButton button)
    {
        if (button == null || selectionHighlight == null || selection == null)// || selection == lastSelection
            return;

        TextMeshProUGUI txt = selection.GetComponentInChildren<TextMeshProUGUI>();
        bool bgs = false;
        if (txt != null)
        {
            string tx = txt.text;
            bgs = tx.Length == 1;
        }
        int playerIndex = GetComponent<RacerData>().index;

        selectionColor = playerIndex switch
        {
            0 => new Color32(255, 25, 25, 255),
            1 => new Color32(50, 200, 50, 255),
            2 => new Color32(255, 255, 0, 255),
            3 => new Color32(0, 190, 255, 255),
            _ => new Color32(255, 255, 255, 255)
        };

        Image highlightImage = selectionHighlight.GetComponent<Image>();
        if (highlightImage == null)
            return;

        highlightImage.color = selectionColor;
        highlightImage.sprite = null;

        if (SceneController.instance.currentSceneType == SceneController.SceneType.MainMenu)
            highlightImage.color = new Color32(180, 180, 180, 255);

        selectionHighlight.gameObject.SetActive(true);

        if (button.transform != null)
            selectionHighlight.transform.SetParent(button.transform.parent);

        if (SceneController.instance.currentSceneType == SceneController.SceneType.MainMenu && bgs)
        {
            highlightImage.sprite = selection.buttonBackground.sprite;
            highlightImage.color = new Color32(180, 180, 180, 255);
        }

        selectionHighlight.transform.SetParent(button.transform.parent);
        UpdateAllHighlights();
    }

    private void UpdateAllHighlights()
    {
        int maxPlayers = playerSelections.Count;

        var grouped = playerSelections.GroupBy(p => p.selection);

        foreach (var group in grouped)
        {
            var sorted = group.OrderByDescending(p => p.GetComponent<RacerData>().index).ToList();

            for (int i = 0; i < sorted.Count; i++)
            {
                UISelection ui = sorted[i];

                if (ui.selection == null || ui.selectionHighlight == null)
                    continue;

                RectTransform highlight = ui.selectionHighlight;
                RectTransform buttonRect = ui.selection.GetComponent<RectTransform>();

                if (buttonRect == null)
                    continue;

                int clampedIndex = Mathf.Clamp(i, 0, maxPlayers - 1);

                float basePadding = 25f;
                float paddingStep = 15f;

                highlight.sizeDelta = buttonRect.sizeDelta + Vector2.one * (basePadding + paddingStep * clampedIndex);

                if (selection.GetComponent<Slider>() != null)
                    highlight.localScale = new Vector3(1.1f, 5, 1);
                else
                    highlight.localScale = Vector3.one;

                highlight.position = buttonRect.position;
                highlight.rotation = Quaternion.identity;
                highlight.SetSiblingIndex(sorted.Count - 1 - i);
            }
        }
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

        if (draggingSlider != null)
        {
            if (!Mouse.current.leftButton.isPressed)
            {
                draggingSlider = null;
                return;
            }

            UpdateSliderValue(draggingSlider);
            return;
        }

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

    public void Skip()
    {
        GameObject controller = GameObject.FindWithTag("Announcement controller");
        string sce = SceneManager.GetActiveScene().name;

        switch (sce)
        {
            case "Level1 Ann.": controller.GetComponent<VideoManager>().SwitchScenes("Level1_sloped"); break;
            case "Level2 Ann.": controller.GetComponent<VideoManager>().SwitchScenes("Level2"); break;
            case "Level3 Ann.": controller.GetComponent<VideoManager>().SwitchScenes("Level3"); break;
        }
    }
}