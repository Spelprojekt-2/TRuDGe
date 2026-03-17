using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class CharacterSpecials : MonoBehaviour
{
    [SerializeField] private LarsMessage lm;
    [SerializeField] private NinaMechanic nm;
    [SerializeField] private NapoleonRespect nr;
    [SerializeField] private ShudderChat sc;
    [SerializeField] private RacerData rd;
    [SerializeField] private TextMeshProUGUI toggleText;
    [SerializeField] private GameObject controller;
    [SerializeField] private GameObject kbm;
    private bool specialsEnabled = true;

    void Start()
    {
        SceneController.instance.SceneChangeEvent += OnSceneLoaded;
    }
    void OnSceneLoaded()
    {
        if (SceneController.instance.IsMenu)
        {
            toggleText.text = "";
            bool isController = transform.root.GetComponentInChildren<PlayerInput>().currentControlScheme == "Gamepad";
            if (isController)
            {
                kbm.SetActive(false);
                controller.SetActive(true);
            }
            else
            {
                kbm.SetActive(true);
                controller.SetActive(false);
            }
        }
        else
        {
            toggleText.text = "Toggle special with:";
            //kbm.SetActive(false);
        }
    }
    void OnDisable()
    {
        SceneController.instance.SceneChangeEvent -= OnSceneLoaded;
    }
    public void ToggleCharacterSpecials()
    {
        if (specialsEnabled)
        {
            switch (rd.racername)
            {
                case "Lars-Göran": lm.toggleMessage(false); break;
                case "The Brass Beast": nm.toggleWrenches(false); break;
                case "King Napoleon III": nr.toggleRespect(false); break;
                case "Capôw": sc.EnableChat(false); break;
            }
            specialsEnabled = false;
        }
        else
        {
            switch (rd.racername)
            {
                case "Lars-Göran": lm.toggleMessage(true); break;
                case "The Brass Beast": nm.toggleWrenches(true); break;
                case "King Napoleon III": nr.toggleRespect(true); break;
                case "Capôw": sc.EnableChat(true); break;
            }
            specialsEnabled = true;
        }
    }
}
