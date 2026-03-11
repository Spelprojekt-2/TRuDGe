using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrainingGroundReady : MonoBehaviour
{
    private bool isOnTrainingGround;
    private bool isReady;
    private static int playersReady;
    [SerializeField] private GameObject TrainingUI;
    [SerializeField] private GameObject PrereadyText;
    [SerializeField] private GameObject ReadyText;
    [SerializeField] private GameObject KBMInputs;
    [SerializeField] private GameObject ControllerInputs;
    private void Start()
    {
        SceneController.instance.SceneChangeEvent += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneController.instance.SceneChangeEvent -= OnSceneLoaded;
    }

    private void OnSceneLoaded()
    {
        isOnTrainingGround = SceneController.instance.currentSceneType == SceneController.SceneType.TrainingGround;
        isReady = false;
        playersReady = 0;
        if (TrainingUI == null) return;
        TrainingUI.SetActive(isOnTrainingGround);
        PrereadyText.SetActive(isOnTrainingGround);
        ReadyText.SetActive(false);
        if (isOnTrainingGround)
        {
            bool isController = GetComponent<PlayerInput>().currentControlScheme == "Gamepad";
            if (isController)
            {
                KBMInputs.SetActive(false);
                ControllerInputs.SetActive(true);
            }
            else
            {
                KBMInputs.SetActive(true);
                ControllerInputs.SetActive(false);
            }
        }
        else
        {
            KBMInputs.SetActive(false);
            ControllerInputs.SetActive(false);
        }
        if (SceneController.instance.currentSceneType == SceneController.SceneType.TrainingGround)
        {
            //GameObject mapicon = GameObject.Find("MinimapContainer");
            //mapicon.GetComponent<MinimapIcons>().enabled = false;

            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject p in players)
            {
                GameObject lapimage = GameObject.Find("LapImage");
                lapimage.GetComponent<Image>().enabled = false;

                GameObject posimage = GameObject.Find("PositionImage");
                posimage.GetComponent<Image>().enabled = false;
            }
        }
        if (SceneController.instance.currentSceneType == SceneController.SceneType.Racing)
        {
            //GameObject mapicon = GameObject.Find("MinimapContainer");
            //mapicon.GetComponent<MinimapIcons>().enabled = true;

            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject p in players)
            {
                GameObject lapimage = GameObject.Find("LapImage");
                lapimage.GetComponent<Image>().enabled = true;

                GameObject posimage = GameObject.Find("PositionImage");
                posimage.GetComponent<Image>().enabled = true;
            }
        }
    }

    public void ReadyUp(InputAction.CallbackContext context)
    {
        if (!context.performed || !isOnTrainingGround) return;
        if (isReady)
        {
            isReady = false;
            playersReady--;
            PrereadyText.SetActive(true);
            ReadyText.SetActive(false);
        }
        else
        {
            isReady = true;
            playersReady++;
            PrereadyText.SetActive(false);
            ReadyText.SetActive(true);
            if (playersReady == PlayerTrackerManager.instance.GetPlayerCount())
            {
                SceneManager.LoadScene(RacingInformation.instance.trackToPlay);
            }
        }
    }
}