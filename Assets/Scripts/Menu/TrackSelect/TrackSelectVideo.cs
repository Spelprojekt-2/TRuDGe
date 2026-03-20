using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;
using UnityEngine.EventSystems;

public class TrackSelectVideo : MonoBehaviour
{
    [SerializeField] private VideoPlayer[] videos;
    [SerializeField] private Image img;
    int lastVideo = -1;
    void Start()
    {
        //if (b.currentSelectionState == SelectionState.Selected)
        //img.color = new Color32(0,0,0,255);
    }
    /*public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("AAAAAAAAAAAAAAA");
    }*/
    public void ChangeVideo()
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
                videos[i].time += time;
            }    
            else
            {
                videos[i].Stop();
            }
                
        }
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
    }
}
