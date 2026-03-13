using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.InputSystem;
using TMPro;

public class VideoManager : MonoBehaviour
{
    [Header ("Video")]
    public VideoPlayer video;

    [Header("Subtitles")]
    public SubtitlesManager submanager;

    [Header("Scene")]
    [SerializeField] private string scenee = "Level1_sloped";
    
    void Start()
    {
        string nam = SceneManager.GetActiveScene().name.ToLower();
        switch (nam)
        {
            case "level1 ann.": AudioManager.Instance.PlaySchlammenstreckeIntro(); 
            submanager.SchlammenstreckeIntro(); break;
            case "level2 ann.": AudioManager.Instance.PlayCliffsOfDoverIntro();
            submanager.CliffsOfDoverIntro(); break;
            case "level3 ann.": AudioManager.Instance.PlayLuminenTRTIntro();
            submanager.LuminenTRTIntro(); break;
        }
        video.Play();
        video.loopPointReached += SwapVideo;
        }

    public void SwapVideo(VideoPlayer vp)
    {
        SwitchScenes(scenee);
    }
    public void SwitchScenes(string scene)
    {
        AudioManager.Instance.StopIntro();
        SceneManager.LoadScene(scene);
    }
    void OnDestroy()
    {
        video.loopPointReached -= SwapVideo;
    }
}
