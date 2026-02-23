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
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void SetGhostFile(string fullPath, bool isTimeTrialGhost)
    {
        pathToGhost = fullPath;
        isTimeTrialWithGhost = isTimeTrialGhost;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        switch (scene.name)
        {
            case "TrackSelectTimeTrial":
                isTimeTrialWithGhost = false;
                break;
            case "SelectionScreen":
                isTimeTrial = false;
                break;
            case "TimeTrialMenu":
                isTimeTrial = true;
                break;
        }
    }

    public void SetTrack(string name)
    {
        trackToPlay = name;
    }
}
