using UnityEngine;
using TMPro;
using System.Collections.Generic;


public class ShudderChat : MonoBehaviour
{
    public GameObject chatPref;
    public Transform chatPar;
    public float SpaceY = 10f;
    public float bottomOffs = 50f;
    private List<GameObject> activeChat = new List<GameObject>();
    void Awake()
    {
        
    }

    void SpawnChat(string chattext, float duration)
    {
        GameObject newChat =
            Instantiate(chatPref, chatPar, false);
        newChat.SetActive(true);
        TextMeshProUGUI text =
            newChat.GetComponentInChildren<TextMeshProUGUI>();

        text.text = chattext;

        text.ForceMeshUpdate();

        RectTransform textRect = text.GetComponent<RectTransform>();
        RectTransform bgRect = newChat.GetComponent<RectTransform>();

        float paddingX = 20f;
        float paddingY = 10f;

        bgRect.sizeDelta = new Vector2(
            text.preferredWidth + paddingX,
            text.preferredHeight + paddingY);

        float yPos = bottomOffs;

        foreach (GameObject chat in activeChat)
        {
            RectTransform r = chat.GetComponent<RectTransform>();
            yPos += r.sizeDelta.y + SpaceY;
        }

        bgRect.anchoredPosition = new Vector2(0f, yPos);

        activeChat.Add(newChat);

        Destroy(newChat, duration);
        StartCoroutine(RemoveAfter(newChat, duration));
    }

    System.Collections.IEnumerator RemoveAfter(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);

        activeChat.Remove(obj);
        RearrangeChat();
    }

    void RearrangeChat()
    {
        float yPos = bottomOffs;

        foreach (GameObject chat in activeChat)
        {
            if (chat == null) continue;

            RectTransform r = chat.GetComponent<RectTransform>();
            r.anchoredPosition = new Vector2(0f, yPos);

            yPos += r.sizeDelta.y + SpaceY;
        }
    }

    //======= Chat =======
    public void CringeChat()
    {
        //Chatter name
        string ChatterName = "K-nuckles";
        int ChatsterName = Random.Range(1, 3);
        switch (ChatsterName)
        {
            case 1: ChatterName = "Buddha: "; break;
            case 2: ChatterName = "Juggler: "; break;
        }

        //Chatter color
        /*int ColorR = Random.Range(0, 256);
        int ColorB = Random.Range(0, 256);
        int ColorG = Random.Range(0, 256);
        int ColorA = 255;
        ChatterNameColor = new Color32(ColorR, ColorB, ColorG, ColorA);*/


        //ChatterName.Colo
        

        /*int ChatsterColor = Random.Range(1, 10);
        switch (ChatsterColor)
        {
            case 1: ChatterName.color = new Color32(Random.Range(0, 256), Random.Range(0, 256), Random.Range(0, 256), 255);
            case 2: ChatterName = "Juggler";
        }ColorUtility.ToHtmlStringRGB(ChatterNameColor)+*/
            
        //Chatter message
        string ChatMessage = "OWKDLDAFIKSDOMVINSIEA";
        int ChatsterMessage = Random.Range(1, 5);
        switch (ChatsterMessage)
        {
            case 1: ChatMessage = "Cringe"; break;
            case 2: ChatMessage = "CringeAlert"; break;
            case 3: ChatMessage = "Sminge"; break;
            case 4: ChatMessage = "Nawur"; break;
        }
        SpawnChat(ChatterName + ChatMessage, 20f);
        //Invoke(nameof(Announcement_Intro1), 5f);
    }
}
