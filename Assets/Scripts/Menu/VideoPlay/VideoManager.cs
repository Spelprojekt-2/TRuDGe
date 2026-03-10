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

    //[Header("Subtitles")]
    //public TextMeshProUGUI subtitlesText;

    [Header("Scene")]
    [SerializeField] private string scenee = "Level1_sloped";
    
    void Start()
    {
        /*PlayerCamera[] cam = FindObjectsOfType<PlayerCamera>();
        //GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        //PlayerCamera[] cam = players.GetComponentInChildren<PlayerCamera>()
        foreach (PlayerCamera comp in cam)
        {
            //comp.enabled = false;
            Transform player = comp.transform.Find("CameraHolder");
            if (player != null)
            {
                player.gameObject.SetActive(false);
            }
        }*/

        voiceAudio.PlayAnnouncement();
        video.Play();
        video.loopPointReached += SwapVideo;

        /*SceneManager.LoadSceneAsync("Level1_sloped");
        SceneManager.LoadSceneAsync("Level2");
        SceneManager.LoadSceneAsync("Level3");*/
        //subtitlesText.text = "Schlammrennstrecke, a mud track located in a beautiful German beech forest. It features tight turns, a few tricky obstacles and ending off with another right turn toward the goal. Schlammrennstrecke was originally designed for drifting with cars but unfortunately most of it was destroyed during “The Battle of Schlamm” in the war. In 2055 it was repaired, forming the amazing track we see today!";
    }

    public void SwapVideo(VideoPlayer vp)
    {
        SwitchScenes(scenee);
    }
    public void SwitchScenes(string scene)
    {
        /*PlayerCamera[] cam = FindObjectsOfType<PlayerCamera>();
        //GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        
        foreach (PlayerCamera comp in cam)
        {
            //comp.enabled = true;
            Transform player = comp.transform.Find("CameraHolder");
            if (player != null)
            {
                player.gameObject.SetActive(true);
            }
        }*/
    
        SceneManager.LoadScene(scene);
    }


    void OnDestroy()
    {
        video.loopPointReached -= SwapVideo;
    }
}
