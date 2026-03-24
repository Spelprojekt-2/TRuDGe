using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;

public class MainMenuVideos : MonoBehaviour
{
    [SerializeField] private VideoPlayer[] videos;
    [SerializeField] private Image img;
    int lastVideo = -1;
    void Start()
    {
        InvokeRepeating(nameof(ChangeVideo),0f, 15f);
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
            {
                
                float time = Random.Range(0, (float)videos[i].length - 17);
                videos[i].Play();
                videos[i].StepForward();
                videos[i].time += time;
                StartCoroutine(FadeIn());
                StartCoroutine(FadeOut(time));
            }    
            else
            {
                
                videos[i].Stop();
            }
        }
    }
    private IEnumerator FadeOut(float time)
    {
        yield return new WaitForSeconds(14f);
        for (int i = 148; i<256; i++)
        {
            img.color = new Color32(0,0,0,(byte)i);
            yield return new WaitForSeconds(0.005f);
        }
    }
    private IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(0.3f);
        for (int i = 255;i>148;i--)
        {
            img.color = new Color32(0,0,0,(byte)i);
            yield return new WaitForSeconds(0.007f);
        }
    }
}
