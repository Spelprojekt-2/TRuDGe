using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ChangeGrappleSprite : MonoBehaviour
{
    [SerializeField] private Image gindicator;
    [SerializeField] private Sprite controller;
    [SerializeField] private Sprite kbm;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SceneController.instance.SceneChangeEvent += OnSceneLoaded;
    }
    void OnSceneLoaded()
    {
        if (!SceneController.instance.IsMenu)
        {
            bool isController = transform.root.GetComponentInChildren<PlayerInput>().currentControlScheme == "Gamepad";
            if (isController)
            {
                gindicator.sprite = controller;
            }
            else
            {
                gindicator.sprite = kbm;
            }
        }
    }
    void OnDisable()
    {
        SceneController.instance.SceneChangeEvent -= OnSceneLoaded;
    }
}
