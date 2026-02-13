using System;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    #region Configuration
    [Tooltip("List of bus paths to pause, e.g., 'bus:/SFX', 'bus:/Music'")]
    [SerializeField] private List<string> busesToMute;

    [SerializeField] private EventReference Music_MainMenuRef;
    [SerializeField] private EventReference Music_SelectionScreenRef;
    [SerializeField] private EventReference Music_TimeTrailRef;
    [SerializeField] private EventReference Music_Level1_slopedRef;

    private EventInstance musicInstance;

    public enum MusicID
    {
        MainMenu = 0,
        SelectionScreen = 1,
        TimeTrail = 2,
        Level1sloped = 3
    }
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        ChangeMusic(MusicID.MainMenu);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #region Pause / Resume

    public void PauseAudio()
    {
        TogglePause(true);
        Debug.Log("Audio paused");
    }

    public void ResumeAudio()
    {
        TogglePause(false);
        Debug.Log("Audio resumed");
    }

    private void TogglePause(bool pause)
    {
        foreach (string busPath in busesToMute)
        {
            Bus bus = RuntimeManager.GetBus(busPath);

            if (bus.isValid())
                bus.setPaused(pause);
            else
                Debug.LogWarning($"Bus not found: {busPath}");
        }
    }

    #endregion

    #region Music

    public void ChangeMusic(MusicID musicID)
    {
        if (musicInstance.isValid())
        {
            musicInstance.stop(STOP_MODE.IMMEDIATE);
            musicInstance.release();
        }

        switch (musicID)
        {
            case MusicID.MainMenu:
                musicInstance = RuntimeManager.CreateInstance(Music_MainMenuRef);
                break;

            case MusicID.SelectionScreen:
                musicInstance = RuntimeManager.CreateInstance(Music_SelectionScreenRef);
                break;

            case MusicID.TimeTrail:
                musicInstance = RuntimeManager.CreateInstance(Music_TimeTrailRef);
                break;

            case MusicID.Level1sloped:
                musicInstance = RuntimeManager.CreateInstance(Music_Level1_slopedRef);
                break;
        }

        musicInstance.start();
    }

    #endregion

    #region Scene Handling

    private void OnSceneLoaded(Scene next, LoadSceneMode mode)
    {
        switch (next.name)
        {
            case "MainMenu":
                ChangeMusic(MusicID.MainMenu);
                break;

            case "SelectionScreen":
                ChangeMusic(MusicID.SelectionScreen);
                break;

            case "TimeTrial":
                ChangeMusic(MusicID.TimeTrail);
                break;

            case "Level1_sloped":
                ChangeMusic(MusicID.Level1sloped);
                break;
        }
    }
    #endregion
}