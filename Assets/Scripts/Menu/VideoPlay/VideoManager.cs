using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.InputSystem;
using TMPro;

public class VideoManager : MonoBehaviour
{
    [Header ("Video")]
    public VideoPlayer video;
    [SerializeField] private VoiceAudio voiceAudio;

    [Header("Subtitles")]
    //public TextMeshProUGUI subtitlesText;

    [Header("Scene")]
    public string SwitchScene = "Level1_sloped";

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        {
            //SwitchScenes();
        }
    }
    
    void Start()
    {
        voiceAudio.PlayAnnouncement();
        video.Play();
        video.loopPointReached += SwapVideo;

        //subtitlesText.text = "Schlammrennstrecke, a mud track located in a beautiful German beech forest. It features tight turns, a few tricky obstacles and ending off with another right turn toward the goal. Schlammrennstrecke was originally designed for drifting with cars but unfortunately most of it was destroyed during “The Battle of Schlamm” in the war. In 2055 it was repaired, forming the amazing track we see today!";
    }

    public void SwapVideo(VideoPlayer vp)
    {
        SwitchScenes();
    }
    public void SwitchScenes()
    {
        SceneManager.LoadScene(SwitchScene);
    }

    void OnDestroy()
    {
        video.loopPointReached -= SwapVideo;
    }
}
