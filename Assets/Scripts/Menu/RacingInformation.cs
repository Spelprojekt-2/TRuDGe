using UnityEngine;
using UnityEngine.SceneManagement;

public class RacingInformation : MonoBehaviour
{
    public static RacingInformation instance;
    public string trackToPlay;
    public bool isTimeTrial = false;
    public bool isTimeTrialWithGhost = false;
    public string pathToGhost;
    void Start()
    {
        instance = this;
        SceneController.instance.SceneChangeEvent -= OnSceneLoaded;
        SceneController.instance.SceneChangeEvent += OnSceneLoaded;
    }

    public void SetGhostFile(string fullPath, bool isTimeTrialGhost)
    {
        pathToGhost = fullPath;
        isTimeTrialWithGhost = isTimeTrialGhost;
    }

    private void OnSceneLoaded()
    {
        switch (SceneController.instance.currentSceneType)
        {
            case SceneController.SceneType.TrackSelectTimeTrial:
                isTimeTrialWithGhost = false;
                break;
            case SceneController.SceneType.PlayerSelectRace:
                isTimeTrial = false;
                break;
            case SceneController.SceneType.PlayerSelectTimeTrial:
                isTimeTrial = true;
                break;
        }
    }

    public void SetTrack(string name)
    {
        trackToPlay = name;
    }
}