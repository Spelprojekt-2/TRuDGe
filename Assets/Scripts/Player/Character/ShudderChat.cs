using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;


public class ShudderChat : MonoBehaviour
{
    public GameObject chatPref;
    public Transform chatPar;
    public float SpaceY = 3f;
    public float bottomOffs = 50f;
    private List<GameObject> activeChat = new List<GameObject>();
    [SerializeField] private Image phoneImage;

    //Chatstuf
    void Awake()
    {
        //ChatWin();
    }

    //Chatter name
        private string[] chatterNames =
        {
            "Buddha",
            "Juggler",
            "K-nuckles",
            "Nutslack2020",
            "Tanker",
            "T-lover",
            "Corkscrew",
            "Lars-Göran_II",
            "GothNina",
            "TheKingofFrance",
            "SchlammQueen",
            "Skadoosh",
            "TCOINFUTURE",
            "gOLDFISmEMORY",
            "ClowningHard",
            "memoryleak",
            "RockManager",
            "PoE6.967",
            "OLOY",
            "NoFansMngr",
            "Helenanananan",
            "MagganLöfv",
            "CutinisRight",
            "Trudgeinator",
            "WhereHerbert",
            "GruppSexBad",
            "yourewelcome",
            "Death",
            "SKULLEMOJI",
            "Mouski>Control",
            "SandwichTable",
            "ILOVESCHLORSH",
            "Microslop",
            "UBIMAD",
            //gruppens dc
            "Massiv",
            "arrerino",
            "Bones",
            "Honey",
            "Kiosotto",
            "trustworthy",
            "spotunna",
            "HUMLAN34",
            "Rubin",
            "Sage",
            "scxeed",
            "Steelwrecker",
            "Herbert",
            "Steve"
        };
        //Chatter color
        private string[] colors =
        {
            "red",
            "green",
            "blue",
            "yellow",
            "#00ffffff",
            "#a52a2aff",
            "#0000a0ff",
            "#00ff00ff",
            "#ffa500ff",
            "#c0c0c0ff",
            "#008080ff"
        };
    void Update()
    {
        int chatSpawn = Random.Range(1, 360);
        if (chatSpawn == 1)
        {
            CringeChat();
        }
    }

    void SpawnChat(string chattext, float duration)
    {
        GameObject newChat = Instantiate(chatPref, chatPar, false);
        newChat.SetActive(true);

        TextMeshProUGUI text = newChat.GetComponentInChildren<TextMeshProUGUI>();
        text.text = chattext;

        text.ForceMeshUpdate();

        RectTransform textRect = text.GetComponent<RectTransform>();
        RectTransform bgRect = newChat.GetComponent<RectTransform>();

        float paddingX = 0f;
        float paddingY = 0f;

        bgRect.sizeDelta = new Vector2(
            text.preferredWidth + paddingX,
            text.preferredHeight + paddingY);

        float yPos = bottomOffs;

        foreach (GameObject chat in activeChat)
        {
            RectTransform r = chat.GetComponent<RectTransform>();
            yPos += r.sizeDelta.y + SpaceY;
        }

        bgRect.anchoredPosition = new Vector2(100f, yPos + -150f);

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
            r.anchoredPosition = new Vector2(100f, yPos - 150f);

            yPos += r.sizeDelta.y + SpaceY;
        }
    }

    //======= Chat =======
    public void CringeChat()
    {
        int randomChatter = Random.Range(0, chatterNames.Length * colors.Length);

        int nameIndex = randomChatter / colors.Length;
        int colorIndex = randomChatter % colors.Length;

        string ChatterName = $"<color={colors[colorIndex]}>{chatterNames[nameIndex]}:</color> ";

        //Chatter message
        string[] ChatMessages =
        {
            "CAPÔW",
            "Cringe",
            "Sminge",
            "TANKS ARE AWESOME",
            "Go go go!",
            "Bullen>Gabriel",
            "Lets go",
            "TRUDGE!",
            "F",
            "Ultimatus",
            "sign my forehead pls",
            "2060 is the new 2020",
            "Cars 8 > Cars 1",
            "WHERE IS HERBERT",
            "Napoleon for king!!",
            "what are you doing",
            "Speed up",
            "go faster",
            "#ILOVESCHLORSCH",
            "Much better than cars",
            "Beef nugget is life",
            "Ghiniwhini is faster",
            "BOSS engines my goat",
            "Just won 50k @WinZone",
            "chug speedrunner =win",
            "GameStation 7 is so good",
            "ILY CARLA"
        };

        int randomMessage = Random.Range(0, ChatMessages.Length);
        string ChatMessage = $"{ChatMessages[randomMessage]}";
        SpawnChat(ChatterName + ChatMessage, 6f);
    }

    //Scripted chat
    public void ChatDistract()
    {
        SpawnChat("<color=green>Ghrash:</color> Imma go distract the other racers", 10f);
    }
    public void ChatStreamerNo()
    {
        SpawnChat("<color=#c0c0c0ff>FreakBob:</color> MY STREAMER NOOOOOOO", 10f);
    }
    public void ChatWin()
    {
        int randomChatter = Random.Range(0, chatterNames.Length * colors.Length);

        int nameIndex = randomChatter / colors.Length;
        int colorIndex = randomChatter % colors.Length;

        string ChatterName = $"<color={colors[colorIndex]}>{chatterNames[nameIndex]}:</color> ";

        SpawnChat(ChatterName + "CHICKEN DINNER!!!", 6f);
    }

    public void EnableChat(bool state)
    {
        GetComponent<ShudderChat>().enabled = state;
        phoneImage.enabled = state;
    }

    //press Y to disable?
}
