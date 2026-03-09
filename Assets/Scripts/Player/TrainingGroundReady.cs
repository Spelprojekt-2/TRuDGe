using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TrainingGroundReady : MonoBehaviour
{
    private bool isOnTrainingGround;
    private bool isReady;
    private static int playersReady;
    [SerializeField] private GameObject TrainingUI;
    [SerializeField] private GameObject PrereadyText;
    [SerializeField] private GameObject ReadyText;
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