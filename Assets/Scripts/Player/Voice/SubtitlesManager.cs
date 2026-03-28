using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class SubtitlesManager : MonoBehaviour
{
    public GameObject subtitlePrefab;
    public Transform subtitleParent;
    public float verticalSpacing = 10f;
    public float bottomOffset = 50f;
    private List<GameObject> activeSubtitles = new List<GameObject>();

    void Awake()
    {

    }

    void SpawnSubtitle(string subtext, float duration)
    {
        GameObject newSubtitle =
            Instantiate(subtitlePrefab, subtitleParent, false);
        newSubtitle.SetActive(true);
        TextMeshProUGUI text =
            newSubtitle.GetComponentInChildren<TextMeshProUGUI>();

        text.text = subtext;

        text.ForceMeshUpdate();

        RectTransform textRect = text.GetComponent<RectTransform>();
        RectTransform bgRect = newSubtitle.GetComponent<RectTransform>();

        float paddingX = 20f;
        float paddingY = 10f;

        bgRect.sizeDelta = new Vector2(
            text.preferredWidth + paddingX,
            text.preferredHeight + paddingY);

        float yPos = bottomOffset;

        foreach (GameObject sub in activeSubtitles)
        {
            RectTransform r = sub.GetComponent<RectTransform>();
            yPos += r.sizeDelta.y + verticalSpacing;
        }

        bgRect.anchoredPosition = new Vector2(0f, yPos);

        activeSubtitles.Add(newSubtitle);

        Destroy(newSubtitle, duration);
        StartCoroutine(RemoveAfterTime(newSubtitle, duration));
    }

    System.Collections.IEnumerator RemoveAfterTime(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);

        activeSubtitles.Remove(obj);
        RearrangeSubtitles();
    }

    void RearrangeSubtitles()
    {
        float yPos = bottomOffset;

        foreach (GameObject sub in activeSubtitles)
        {
            if (sub == null) continue;

            RectTransform r = sub.GetComponent<RectTransform>();
            r.anchoredPosition = new Vector2(0f, yPos);

            yPos += r.sizeDelta.y + verticalSpacing;
        }
    }

    //======= Subtitles =======
    //Announcer
    public void Announcement_Intro()
    {
        SpawnSubtitle("<color=yellow>[</color><color=#5e9cff>Announcer</color><color=yellow>]</color> In the year 2060 another few races will take place all over Europe in the", 6f);
        Invoke(nameof(Announcement_Intro1), 5f);
    }
    void Announcement_Intro1()
    {                  
        SpawnSubtitle("beautiful sport tank rally. Schlammrennstrecke and the Cliffs of Dover are just two", 7f);
        Invoke(nameof(Announcement_Intro2), 6f);
    }
    void Announcement_Intro2()
    {
        SpawnSubtitle("of the exciting, revamped tracks we have this year. 2060 also marks an important", 7f);
        Invoke(nameof(Announcement_Intro3), 6f);
    }
    void Announcement_Intro3()
    {
        SpawnSubtitle("milestone in our world's healing process, a decade has passed since the third world", 7f);
        Invoke(nameof(Announcement_Intro4), 6f);
    }
    void Announcement_Intro4()
    {
        SpawnSubtitle("war ended. Together, we shall continue to work toward a happier world, a world with", 7f);
        Invoke(nameof(Announcement_Intro5), 6f);
    }
    void Announcement_Intro5()
    {
        SpawnSubtitle("<color=red>T</color><color=orange>A</color><color=green>N</color><color=yellow>K</color> <color=red>R</color><color=orange>A</color><color=green>L</color><color=yellow>L</color><color=red>Y</color><color=orange>!</color>", 5f);
    }
    //Schlamm
    public void SchlammenstreckeIntro()
    {
        SpawnSubtitle("<color=yellow>[</color><color=#5e9cff>Herbert</color><color=yellow>]</color> Schlammrennstrecke, a mud track located here in western Germany. It", 6f);
        Invoke(nameof(SchlammenstreckeIntro1), 5f); 
    }
    void SchlammenstreckeIntro1()
    {
        SpawnSubtitle("features a lot of tight turns that are going to force the racers to be very precise", 6f);
        Invoke(nameof(SchlammenstreckeIntro2), 4f); 
    }
    void SchlammenstreckeIntro2()
    {
        SpawnSubtitle("if they want to stay at top speed constantly.", 5f);
        Invoke(nameof(SchlammenstreckeIntro3), 3f); 
    }
    void SchlammenstreckeIntro3()
    {
        SpawnSubtitle("<color=yellow>[</color><color=#5e9cff>Ozzie</color><color=yellow>]</color> Schlammrennstrecke is a track with a lot of history, as it was a famous", 6f);
        Invoke(nameof(SchlammenstreckeIntro4), 5f); 
    }
    void SchlammenstreckeIntro4()
    {
        SpawnSubtitle(" dirt rally track before the war. In 2055 it was repaired and expanded to accomodate", 6f);
        Invoke(nameof(SchlammenstreckeIntro5), 5f); 
    }
    void SchlammenstreckeIntro5()
    {
        SpawnSubtitle(" the new vehicles, forming the amazing track we see today!", 6f);
    }
    //Cliffs
    public void CliffsOfDoverIntro()
    {
        SpawnSubtitle("<color=yellow>[</color><color=#5e9cff>Ozzie</color><color=yellow>]</color> Welcome everyone, to the Cliffs of Dover! These beautiful cliffs have", 6f);
        Invoke(nameof(CliffsOfDoverIntro1), 4.5f);
    }
    public void CliffsOfDoverIntro1()
    {
        SpawnSubtitle(" existed long before humans set foot in Britain, and they will probably remain long", 8f);
        Invoke(nameof(CliffsOfDoverIntro2), 5f);
    }
    public void CliffsOfDoverIntro2()
    {
        SpawnSubtitle(" after. As most of you know, the town of Dover saw some of the heaviest fighting", 8f);
        Invoke(nameof(CliffsOfDoverIntro3), 5.5f);
    }
    public void CliffsOfDoverIntro3()
    {
        SpawnSubtitle(" during the war, and I can only assume that we are going to see some of that fighting", 6f);
        Invoke(nameof(CliffsOfDoverIntro4), 4f);
    }
    public void CliffsOfDoverIntro4()
    {
        SpawnSubtitle(" spirit come through today!", 4f);
        Invoke(nameof(CliffsOfDoverIntro5), 2.5f);
    }
    public void CliffsOfDoverIntro5()
    {
        SpawnSubtitle("<color=yellow>[</color><color=#5e9cff>Herbert</color><color=yellow>]</color> Nowadays the town has been mostly rebuilt, and the fields outside the", 6f);
        Invoke(nameof(CliffsOfDoverIntro6), 3.5f);
    }
    public void CliffsOfDoverIntro6()
    {
        SpawnSubtitle(" town will be the battlegrounds for our competitors today. Starting on the fields", 7f);
        Invoke(nameof(CliffsOfDoverIntro7), 5f);
    }
    public void CliffsOfDoverIntro7()
    {
        SpawnSubtitle(" above the cliffs, the drivers will race along the old road, until a spectacular", 7f);
        Invoke(nameof(CliffsOfDoverIntro8), 5f);
    }
    public void CliffsOfDoverIntro8()
    {
        SpawnSubtitle(" drop down to the beach. There, they will drive along the beachhead, dodging", 7f);
        Invoke(nameof(CliffsOfDoverIntro9), 5.5f);  
    }
    public void CliffsOfDoverIntro9()
    {
        SpawnSubtitle(" fortifications from the war before ascending to the fields once again!", 7f);
    }
    //Luminen
    public void LuminenTRTIntro()
    {
        SpawnSubtitle("<color=yellow>[</color><color=#5e9cff>Ozzie</color><color=yellow>]</color> Ladies and gentlemen, welcome to Luminen TRT! Our racers are currently", 6f);
        Invoke(nameof(LuminenTRTIntro1), 5.5f);
    }
    public void LuminenTRTIntro1()
    {
        SpawnSubtitle(" warming up their engines here, and they better, because it is currently -36 degrees", 6f);
        Invoke(nameof(LuminenTRTIntro2), 5.5f);
    }
    public void LuminenTRTIntro2()
    {
        SpawnSubtitle(" celsius outside! Luckily for them, their vehicles were built to operate in almost", 6f);
        Invoke(nameof(LuminenTRTIntro3), 5.5f);
    }
    public void LuminenTRTIntro3()
    {
        SpawnSubtitle(" all conditions. The Finnish forests are more hostile than most though, which you", 6f);
        Invoke(nameof(LuminenTRTIntro4), 5f);
    }
    public void LuminenTRTIntro4()
    {
        SpawnSubtitle(" can see in the many destroyed vehicles around the track. Those aren’t former racers", 6f);
        Invoke(nameof(LuminenTRTIntro5), 5.5f);
    }
    public void LuminenTRTIntro5()
    {
        SpawnSubtitle(" though as they were destroyed 14 years ago!", 6f);
        Invoke(nameof(LuminenTRTIntro6), 3.7f);
    }
    public void LuminenTRTIntro6()
    {
        SpawnSubtitle("<color=yellow>[</color><color=#5e9cff>Herbert</color><color=yellow>]</color> This winter-only track is going to take our racers over frozen lakes", 6f);
        Invoke(nameof(LuminenTRTIntro7), 4.5f);
    }
    public void LuminenTRTIntro7()
    {
        SpawnSubtitle(" and through dense forests. Let’s hope for their sake that they don’t end up falling", 6f);
        Invoke(nameof(LuminenTRTIntro8), 4.5f);
    }
    public void LuminenTRTIntro8()
    {
        SpawnSubtitle(" through the ice!", 6f);
    }
    //[Announcer] In the year 2060 another few races will take place all over Europe in the
    //features a lot of tight turns that are going to force the racers to be very precise
    //
    //Lars-Göran
    public void Lars_Start()
    {
        SpawnSubtitle("<color=yellow>[</color><color=#5e9cff>Lars-Göran</color><color=yellow>]</color> I hope no one decides to cheat today!", 5f);
    }

    //Napoleon
    public void Napoleon_DriveBy()
    {
        SpawnSubtitle("<color=yellow>[</color><color=#5e9cff>Napoleon</color><color=yellow>]</color> Napoleon drive-by", 5f);
    }
    public void Napoleon_DrivenBy()
    {
        SpawnSubtitle("<color=yellow>[</color><color=#5e9cff>Napoleon</color><color=yellow>]</color> Napoleon driven by", 5f);
    }
    public void Napoleon_Carla_Crash()
    {
        SpawnSubtitle("<color=yellow>[</color><color=#5e9cff>Carla</color><color=yellow>]</color> Go away!", 5f);
        Invoke(nameof(NapoleonReply_Carla_Crash), 3f);
    }
    void NapoleonReply_Carla_Crash()
    {
        SpawnSubtitle("<color=yellow>[</color><color=#5e9cff>Napoleon</color><color=yellow>]</color> No u.", 3f);
    }

    //Nina, Brass beast
    public void Nina_Hit()
    {
        SpawnSubtitle("<color=yellow>[</color><color=#5e9cff>Brass beast</color><color=yellow>]</color> Nina_hit", 5f);
    }

    //Carla, Capôw
    public void Carla_Win()
    {
        SpawnSubtitle("<color=yellow>[</color><color=#5e9cff>Capôw</color><color=yellow>]</color> Carla win", 5f);
    }

    //Leonie, Schlammer

    //André, Dragoș

    //Ragana, Vilkmérgele demon

    //Tristano, Harlequini Martinellini
}