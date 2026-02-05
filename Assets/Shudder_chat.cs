using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Shudder_chat : MonoBehaviour
{
    [Header("UI")]
    public Transform chatContent;
    public GameObject chatMessagePrefab;

    [Header("Timing")]
    public float minMessageDelay = 0.1f;
    public float maxMessageDelay = 1.2f;

    [Header("Limits")]
    public int maxMessages = 50;

    private readonly List<string> usernames = new List<string>()
    {
        "xXDarkLordXx", "pogman", "catEnjoyer", "speedrunner",
        "basedUser", "npc_irl", "monkaS", "StreamerFan"
    };

    private readonly List<string> messages = new List<string>()
    {
        "Pog",
        "LUL",
        "monkaS",
        "WHAT",
        "no way 💀",
        "skill issue",
        "based",
        "Chat is wild",
        "W",
        "L",
        "HELLO CHAT",
        "💀💀💀"
    };

    private Queue<GameObject> spawnedMessages = new Queue<GameObject>();

    void Start()
    {
        StartCoroutine(ChatLoop());
    }

    IEnumerator ChatLoop()
    {
        while (true)
        {
            SpawnMessage();
            yield return new WaitForSeconds(Random.Range(minMessageDelay, maxMessageDelay));
        }
    }

    void SpawnMessage()
    {
        GameObject msg = Instantiate(chatMessagePrefab, chatContent);

        TextMeshProUGUI text = msg.GetComponent<TextMeshProUGUI>();
        text.text = GenerateMessage();

        spawnedMessages.Enqueue(msg);

        if (spawnedMessages.Count > maxMessages)
        {
            Destroy(spawnedMessages.Dequeue());
        }
    }

    string GenerateMessage()
    {
        string user = usernames[Random.Range(0, usernames.Count)];
        string message = messages[Random.Range(0, messages.Count)];
        string color = ColorUtility.ToHtmlStringRGB(RandomColor());

        return $"<color=#{color}>{user}</color>: {message}";
    }

    Color RandomColor()
    {
        return Random.ColorHSV(0f, 1f, 0.6f, 1f, 0.8f, 1f);
    }
}
