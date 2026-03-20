using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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
        isOnTrainingGround = (SceneController.instance.currentSceneType == SceneController.SceneType.TrainingGround || SceneController.instance.currentSceneType == SceneController.SceneType.STrainingGround);
        isReady = false;
        playersReady = 0;
        if (TrainingUI == null) return;
        TrainingUI.SetActive(isOnTrainingGround);
        if (SceneManager.GetActiveScene().name == "SingleplayerTG")
        {
            ReadyText.SetActive(false);
            PrereadyText.SetActive(false);
        }
        else
        {
            PrereadyText.SetActive(SceneController.instance.currentSceneType == SceneController.SceneType.TrainingGround);
            ReadyText.SetActive(false);
        }
        if (isOnTrainingGround)
        {
            bool isController = GetComponent<PlayerInput>().currentControlScheme == "Gamepad";
            if (isController)
            {
                PrereadyText.GetComponent<TextMeshProUGUI>().text = "You are in the Training Ground!\nPress [Select] to race!";
                KBMInputs.SetActive(false);
                ControllerInputs.SetActive(true);
            }
            else
            {
                PrereadyText.GetComponent<TextMeshProUGUI>().text = "You are in the Training Ground!\nPress [Enter] to race!";
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
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject p in players)
            {
                Transform lapimage = p.transform.Find("Canvas/LapImage");
                if(lapimage != null)
                lapimage.GetComponent<Image>().enabled = false;

                Transform posimage = p.transform.Find("Canvas/RaceUI/PositionImage");
                if(posimage != null)
                posimage.GetComponent<Image>().enabled = false;

                Transform timer = p.transform.Find("Canvas/TimeTrialStuff/Timer");
                if(timer != null)
                timer.GetComponent<TextMeshProUGUI>().enabled = false;
            }
        }
        if (SceneController.instance.currentSceneType == SceneController.SceneType.Racing)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject p in players)
            {
                Transform lapimage = p.transform.Find("Canvas/LapImage");
                if(lapimage != null)
                lapimage.GetComponent<Image>().enabled = true;

                Transform posimage = p.transform.Find("Canvas/RaceUI/PositionImage");
                if(posimage != null)
                posimage.GetComponent<Image>().enabled = true;
            }
        }
    }

    public void ReadyUp(InputAction.CallbackContext context)
    {
        if (!context.performed || 
            !(SceneController.instance.currentSceneType == SceneController.SceneType.TrainingGround) || SceneManager.GetActiveScene().name == "SingleplayerTG") 
            return;
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