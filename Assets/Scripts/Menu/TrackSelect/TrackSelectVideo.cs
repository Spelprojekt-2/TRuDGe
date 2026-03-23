using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;
using UnityEngine.EventSystems;

public class TrackSelectVideo : MonoBehaviour
{
    [SerializeField] private Camera[] cams;
    //[SerializeField] private VideoPlayer[] vp;
    //[SerializeField] private Image img;
    void Start()
    {
        /*if (vp == null || vp.Length < 3) return;
        for (int i = 0; i < vp.Length; i++)
        {
            vp[i].loopPointReached += Fade;
        }*/
    }
    public void ChangeVideo(string title)
    {
        if (cams == null || cams.Length < 3) return;
        switch (title)
            {
                case "Schlammrennstrecke": 
                    cams[0].depth = 101;
                    cams[1].depth = 100;
                    cams[2].depth = 100; break;
                case "Cliffs of Dover":
                    cams[1].depth = 101;
                    cams[0].depth = 100;
                    cams[2].depth = 100; break;
                case "Luminen TRT":
                    cams[2].depth = 101;
                    cams[0].depth = 100;
                    cams[1].depth = 100; break;
                default: return; break;
            }
    }
    /*private void Fade(VideoPlayer vp)
    {
        StartCoroutine(FadeOut());
        StartCoroutine(FadeIn());
    }
    public IEnumerator FadeOut()
    {
        for (int i = 148; i<256; i++)
        {
            img.color = new Color32(0,0,0,(byte)i);
            yield return new WaitForSeconds(0.007f);
        }
    }
    public IEnumerator FadeIn()
    {
        for (int i = 255;i>148;i--)
        {
            img.color = new Color32(0,0,0,(byte)i);
            yield return new WaitForSeconds(0.007f);
        }
    }*/
}
