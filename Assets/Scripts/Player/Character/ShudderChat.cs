using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class ShudderChat : MonoBehaviour
{
    [SerializeField] private int maxChatMessgages;
    private List<string> activeChat = new List<string>();
    [SerializeField] private Image phoneImage;
    public bool chatEnabled = false;
    TextMeshProUGUI textObj;
    // ========= Chatstuf ==========
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
            "Group6Bad",
            "yourewelcome",
            "Death",
            "SKULLEMOJI",
            "Mouski>Control",
            "SandwichTable",
            "ILOVESCHLORSH",
            "Microslop",
            "UBIMAD",
            "FREAKBOB",
            "Peo",
            "Nejlikor",
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
        if (chatEnabled && Time.timeScale == 1f)
        {
        int chatSpawn = Random.Range(1, 160);
        if (chatSpawn == 1)
        {
            CringeChat();
        }
        }
    }
    void Start()
    {
        textObj = GetComponentInChildren<TextMeshProUGUI>();
        SceneController.instance.SceneChangeEvent += OnSceneLoaded;
    }
    void OnSceneLoaded()
    {
        if (!chatEnabled && !SceneController.instance.IsMenu)
        {
            RacerData data = transform.root.GetComponentInChildren<RacerData>();
            if (data.racername == "Capôw")
            {
                EnableChat(true);
            }
        }
        if (chatEnabled && SceneController.instance.IsMenu)
        {
            EnableChat(false);
        }
    }
    void OnDisable()
    {
        SceneController.instance.SceneChangeEvent -= OnSceneLoaded;
    }

    void SpawnChat(string chattext)
    {
        activeChat.Add(chattext);
        if (activeChat.Count >= maxChatMessgages)
        {
            activeChat.RemoveAt(0);
        }
        UpdateChat();
    }
    void UpdateChat()
    {
        string text = "";
        for (int i = 0; i < activeChat.Count; i++)
        {
            text += activeChat[i] + '\n';
        }
        if (activeChat.Count > 0) text.Remove(text.Length - 1, 1);
        textObj.text = text;
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
            "ILY CARLA",
            "o7",
            "CRAZY JUMPSCARE!!!!!!!",
            "Didnt know this: 'USA collapsed after 95-year-old president gambled away the entire budget thinking it was their own money'",
            "Formula -1 has nothing on this",
            "hope you win!",
            "Bettwer than mario kart",
            "RAWR",
            "GOTY",
            "Where scrummaster",
            "Find Lars and... and... AND...",
            "SSSSCCCHCCAPÔÔW",
            "AAAAAAAAAAAAAAA",
            "fmod better tan vvice",
            "where toilet",
            "I wish my house still existed",
            "HELP ME",
            "VIV LA FRANCE!!!!!",
            "have you seen the new Rockbob episode?",
            "YO ADULT WOLF DROPPED A NEW EPISODE!!!",
            "when will nina notice me-...",
            "have you played Magic: The Dispersion?",
            "i hate the new hyperY headpone",
            "+46*********",
            "finally caught a stream",
            "stream better than video",
            "Côôpa, when do you intend to win?",
            "trust the process guys",
            "[Message removed by moderator.]",
            "ooh im straight up trudgeing it",
            "Group6 will never lock in",
            "Hey, I'm a small streamer and if you would please give me a shoutout it would help me so much please please please",
            "Is that an airstike or orbital lazer?",
            "Is Lars still single?",
            "We last longer on friday night",
            "Fat explosions",
            "Burn in hell Goffman",
            "its windy out here"
        };

        int randomMessage = Random.Range(0, ChatMessages.Length);
        string ChatMessage = $"{ChatMessages[randomMessage]}";
        SpawnChat(ChatterName + ChatMessage);
    }

    //Scripted chat
    public void ChatDistract()
    {
        SpawnChat("<color=green>Ghrash:</color> Imma go distract the other racers");
    }
    public void ChatStreamerNo()
    {
        SpawnChat("<color=#c0c0c0ff>FreakBob:</color> MY STREAMER NOOOOOOO");
    }
    public void ChatWin()
    {
        int randomChatter = Random.Range(0, chatterNames.Length * colors.Length);

        int nameIndex = randomChatter / colors.Length;
        int colorIndex = randomChatter % colors.Length;

        string ChatterName = $"<color={colors[colorIndex]}>{chatterNames[nameIndex]}:</color> ";

        SpawnChat(ChatterName + "CHICKEN DINNER!!!");
    }

    public void EnableChat(bool state)
    {
        activeChat.Clear();
        UpdateChat();
        chatEnabled = state;
        //phoneImage.enabled = state;
    }

    //press Y to disable?
}