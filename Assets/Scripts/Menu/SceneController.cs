using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    public SceneType currentSceneType;
    public bool IsMenu { get; private set; }
    public event Action SceneChangeEvent;
    private void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoad;
        OnSceneLoad(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "MainMenu":
                currentSceneType = SceneType.MainMenu;
                IsMenu = true; break;
            case "SelectionScreen":
                currentSceneType = SceneType.PlayerSelectRace;
                IsMenu = true; break;
            case "TimeTrialMenu":
                currentSceneType = SceneType.PlayerSelectTimeTrial;
                IsMenu = true; break;
            case "TrackSelect":
                currentSceneType = SceneType.TrackSelectRace;
                IsMenu = true; break;
            case "TrackSelectTimeTrial":
                currentSceneType = SceneType.TrackSelectTimeTrial;
                IsMenu = true; break;
            case "AfterRace":
                currentSceneType = SceneType.PostRaceLeaderboard;
                IsMenu = true; break;
            case "TrainingGround":
                currentSceneType = SceneType.TrainingGround;
                IsMenu = false; break;
             case "SingleplayerTG":
                currentSceneType = SceneType.TrainingGround;
                IsMenu = false; break;
            default:
                currentSceneType = SceneType.Racing;
                IsMenu = false; break;
        }

        CoroutineRunner.Run(SceneChange());
    }

    IEnumerator SceneChange()
    {
        yield return null;
        yield return null;
        SceneChangeEvent?.Invoke();
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public enum SceneType
    {
        MainMenu,
        PlayerSelectRace,
        PlayerSelectTimeTrial,
        TrackSelectRace,
        TrackSelectTimeTrial,
        PostRaceLeaderboard,
        Racing,
        TrainingGround,
    }

}
