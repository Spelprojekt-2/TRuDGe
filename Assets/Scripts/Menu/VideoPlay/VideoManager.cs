using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.InputSystem;
using TMPro;

public class VideoManager : MonoBehaviour
{
    [Header ("Video")]
    public VideoPlayer video;

    //[Header("Subtitles")]
    //public TextMeshProUGUI subtitlesText;

    [Header("Scene")]
    [SerializeField] private string scenee = "Level1_sloped";
    
    void Start()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Level1 Ann.": AudioManager.Instance.PlaySchlammenstreckeIntro(); break;
            case "Level2 Ann.": AudioManager.Instance.PlayCliffsOfDoverIntro(); break;
            case "Level3 Ann.": AudioManager.Instance.PlayLuminenTRTIntro(); break;
        }
        video.Play();
        video.loopPointReached += SwapVideo;

        
       
        
        //subtitlesText.text = "Schlammrennstrecke, a mud track located in a beautiful German beech forest. It features tight turns, a few tricky obstacles and ending off with another right turn toward the goal. Schlammrennstrecke was originally designed for drifting with cars but unfortunately most of it was destroyed during “The Battle of Schlamm” in the war. In 2055 it was repaired, forming the amazing track we see today!";
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
