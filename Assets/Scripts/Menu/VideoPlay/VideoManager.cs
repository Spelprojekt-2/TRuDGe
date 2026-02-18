using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.InputSystem;

public class VideoManager : MonoBehaviour
{
    public VideoPlayer video1;
    public VideoPlayer video2;
    public VideoPlayer video3;
    public VideoPlayer video4;
    public VideoPlayer video5;
    public VideoPlayer video6;
    public AudioSource announcment;

    public string SwitchScene = "Level1_sloped";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchScenes();
        }
    }
    
    void Start()
    {
        announcment.Play();
        video1.Play();
        video1.loopPointReached += SwapVideo1;
    }
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
