using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TrainingGroundReady : MonoBehaviour
{
    private bool isOnTrainingGround;
    private bool isReady;
    private static int playersReady;
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
    }

    public void ReadyUp(InputAction.CallbackContext context)
    {
        if (!context.performed || !isOnTrainingGround) return;
        if (isReady)
        {
            isReady = false;
            playersReady--;
        }
        else
        {
            isReady = true;
            playersReady++;
            if (playersReady == PlayerTrackerManager.instance.GetPlayerCount())
            {
                SceneManager.LoadScene(RacingInformation.instance.trackToPlay);
            }
        }
    }
}