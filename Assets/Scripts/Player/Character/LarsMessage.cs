using UnityEngine;
using TMPro;
using System.Collections;

public class LarsMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI message;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float threshold = 30f;
    [SerializeField] private RacerData rd;
    [SerializeField] private bool messageEnabled;
    private Color clr = new Color32(255, 255, 255, 255);
    private int check = 0;
    
    void Start()
    {
        SceneController.instance.SceneChangeEvent += OnSceneLoaded;
    }
    void OnSceneLoaded()
    {
        if (!messageEnabled && !SceneController.instance.IsMenu)
        {
            if (rd.racername == "Lars-Göran")
            {
                messageEnabled = true;
            }
        }
        if (messageEnabled && SceneController.instance.IsMenu)
        messageEnabled = false;
    }
    void Update()
    {
        float speed = rb.linearVelocity.magnitude;

        if (speed < threshold && messageEnabled && check == 0)
        {
            StartCoroutine(ShowMessage());
            check = 1;
        }
    }
    string[] selfhelpMessages =
    {
        "You can conquer anything!",
        "Focus!",
        "You got this!",
        "Nothing can stop you!",
        "Slow and steady wins the race.",
        "In order to learn, you have to fail.",
        "There is no one that is harder on you than yourself.",
        "Everyone makes mistakes.",
        "Acceptance means valuing imperfections as much as perfections.",
        //Questionable self-help
        "Divorce is the first step towards healing.",
        //Internet
        "And now that you don't have to be perfect, you can be good.",
    };

    IEnumerator ShowMessage()
    {
        message.enableVertexGradient = false;
        int r = Random.Range(0, selfhelpMessages.Length);
        message.text = $"<color=yellow>[</color><color=#5e9cff>Lars</color><color=yellow>]</color> {selfhelpMessages[r]}";
        //
        Color32 clr = new Color32(255, 255, 255, 255);
        message.color = clr;

        for (int i = 255; i >= 0; i--)
        {
            message.color = new Color32(255, 255, 255, (byte)i);
            yield return new WaitForSeconds(0.02f);
        }

        check = 0;
    }
    public void toggleMessage(bool state)
    {
        message.text = "";
        messageEnabled = state;
    }
    void OnDisable()
    {
        SceneController.instance.SceneChangeEvent -= OnSceneLoaded;
    }
}
