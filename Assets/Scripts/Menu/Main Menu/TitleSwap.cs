using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleSwap : MonoBehaviour
{
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
    void Start()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        int r = Random.Range(0, colors.Length);
        string tex = $"<color={colors[r]}>T.R.U.D.G.E</color>";
        text.text = tex;
    }
}
