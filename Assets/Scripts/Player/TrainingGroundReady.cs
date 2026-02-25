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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        isOnTrainingGround = scene.name == "TrainingGround";
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