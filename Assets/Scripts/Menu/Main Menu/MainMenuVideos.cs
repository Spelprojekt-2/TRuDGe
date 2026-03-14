using UnityEngine;
using UnityEngine.Video;

public class MainMenuVideos : MonoBehaviour
{
    [SerializeField] private VideoPlayer[] videos;
    int lastVideo = -1;
    void Start()
    {
        InvokeRepeating(nameof(ChangeVideo),0f, 10f);
    }
    void ChangeVideo()
    {
        int r;
        do
        {
            r = Random.Range(0,videos.Length);
        }
        while (r == lastVideo);
        lastVideo = r;
        for (int i = 0; i < videos.Length; i++)
        {
            if (i == r)
                videos[i].Play();
            else
                videos[i].Stop();
        }
    }
}
