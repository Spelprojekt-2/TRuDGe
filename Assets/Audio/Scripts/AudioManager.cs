using System;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class AudioManager : MonoBehaviour
{
    #region Configuration
    public static AudioManager Instance { get; private set; }

    [Tooltip("List of bus paths to pause, e.g., 'bus:/SFX', 'bus:/Music'")]
    [SerializeField] private List<string> busesToMute;

    // Music config
    [Header("Music Configuration")]
    [SerializeField] private EventReference Music_MainMenuRef;
    [SerializeField] private EventReference Music_SelectionScreenRef;
    [SerializeField] private EventReference Music_TimeTrailRef;
    [SerializeField] private EventReference Music_Level1_slopedRef;
    [SerializeField] private EventReference Music_TrainingGround;
    private EventInstance musicInstance;

    public enum MusicID
    {
        MainMenu = 0,
        SelectionScreen = 1,
        TimeTrail = 2,
        Level1sloped = 3,
        TrainingGround = 4
    }

    // SFX config
    [Header("SFX Configuration")]
    [SerializeField] private FeedbackAudio feedbackAudio;
    private EventInstance countDownInst;

    // Ambience config
    [SerializeField] private EventReference AmbienceManagerRef;
    private EventInstance ambienceInstance;

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
            
            case MusicID.TrainingGround:
                musicInstance = RuntimeManager.CreateInstance(Music_TrainingGround);
                break;
        }

        musicInstance.start();
    }
    #endregion

    #region SFX
    public void PlayCountdownAudio(bool endCountdown = false)
    {
        if (feedbackAudio == null)
        {
            Debug.LogWarning("AudioManager: FeedbackAudio is missing!");
            return;
        }
        feedbackAudio.CountdownAudio(endCountdown);
    }
    #endregion

    #region Ambience
    private void StartAmbience()
    {
        if (ambienceInstance.isValid())
        {
            return;
        }
        ambienceInstance = RuntimeManager.CreateInstance(AmbienceManagerRef);
        ambienceInstance.start();
    }
    #endregion

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

    #region Scene Handling
    private void OnSceneLoaded(Scene next, LoadSceneMode mode)
    {
        Debug.Log(next.buildIndex);
        switch (next.buildIndex)
        {
            case 0:
                ChangeMusic(MusicID.MainMenu);
                break;

            case 1:
                ChangeMusic(MusicID.SelectionScreen);
                break;

            case 2:
            Debug.Log("Should change");
                ChangeMusic(MusicID.SelectionScreen);
                break;

            case 3:
                ChangeMusic(MusicID.SelectionScreen);
                break;

            case 9:
                ChangeMusic(MusicID.Level1sloped);
                StartAmbience();
                break;

            case 10:
                ChangeMusic(MusicID.TrainingGround);
                StartAmbience();
                break;
        }
    }
    #endregion

    #region Player Handling
    public void UpdatePlayerCount(int count)
    {
        RuntimeManager.StudioSystem.setParameterByName("PlayerCount", count);
    }
    #endregion
}