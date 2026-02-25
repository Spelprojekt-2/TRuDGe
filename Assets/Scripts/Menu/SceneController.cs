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

    public void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "MainMenu":
                currentSceneType = SceneType.MainMenu; break;
            case "SelectionScreen":
                currentSceneType = SceneType.PlayerSelectRace; break;
            case "TimeTrialMenu":
                currentSceneType = SceneType.PlayerSelectTimeTrial; break;
            case "TrackSelect":
                currentSceneType = SceneType.TrackSelectRace; break;
            case "TrackSelectTimeTrial":
                currentSceneType = SceneType.TrackSelectTimeTrial; break;
            case "AfterRace":
            default:
                currentSceneType = SceneType.Racing; break;
        }

        IsMenu = currentSceneType != SceneType.Racing;
        CoroutineRunner.Run(SceneChange());
    }

    private void Update()
    {
        Debug.Log(currentSceneType);
        Debug.Log(IsMenu);
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

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
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
    }
}
