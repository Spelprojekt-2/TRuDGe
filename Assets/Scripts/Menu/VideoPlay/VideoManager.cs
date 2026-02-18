using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.InputSystem;
using TMPro;

public class VideoManager : MonoBehaviour
{
    [Header ("Videos")]
    public VideoPlayer video1;
    public VideoPlayer video2;
    public VideoPlayer video3;
    public VideoPlayer video4;
    public VideoPlayer video5;
    public VideoPlayer video6;
    [SerializeField] private VoiceAudio voiceAudio;

    [Header("Subtitles")]
    public TextMeshProUGUI subtitlesText;
    public float timer;
    int subtitleIndex = 0;

    [Header("Scene")]
    public string SwitchScene = "Level1_sloped";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchScenes();
        }
        timer += Time.deltaTime;

        if (timer >= 8f && subtitleIndex == 0)
    {
        subtitlesText.text = "obstacles and ending off with another right turn toward the goal. Schlammrennstrecke was originally designed";
        subtitleIndex = 1;
    }
    else if (timer >= 15f && subtitleIndex == 1)
    {
        subtitlesText.text = "for drifting with cars but unfortunately most of it was destroyed during “The Battle of Schlamm” in the war.";
        subtitleIndex = 2;
    }
    else if (timer >= 22f && subtitleIndex == 2)
    {
        subtitlesText.text = "In 2055 it was repaired, forming the amazing track we see today!";
        subtitleIndex = 3;
    }
    }

    void Start()
    {
        voiceAudio.PlayAnnouncement();
        video1.Play();
        video1.loopPointReached += SwapVideo1;
    }

    //Swapping videos
    public void SwapVideo1(VideoPlayer vp)
    {
        video1.gameObject.SetActive(false);
        video2.Play();
        video2.loopPointReached += SwapVideo2;
    }
    public void SwapVideo2(VideoPlayer vp)
    {
        video2.gameObject.SetActive(false);
        video3.Play();
        video3.loopPointReached += SwapVideo3;
    }
    public void SwapVideo3(VideoPlayer vp)
    {
        video3.gameObject.SetActive(false);
        video4.Play();
        video4.loopPointReached += SwapVideo4;
    }
    public void SwapVideo4(VideoPlayer vp)
    {
        video4.gameObject.SetActive(false);
        video5.Play();
        video5.loopPointReached += SwapVideo5;
    }
    public void SwapVideo5(VideoPlayer vp)
    {
        video5.gameObject.SetActive(false);
        video6.Play();
        video6.loopPointReached += SwapVideo6;
    }
    public void SwapVideo6(VideoPlayer vp)
    {
        video6.gameObject.SetActive(false);
        SwitchScenes();
    }

    public void SwitchScenes()
    {
        SceneManager.LoadScene(SwitchScene);
    }
}
