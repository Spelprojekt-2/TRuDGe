using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleSwap : MonoBehaviour
{
    [SerializeField] private Image image;
    private Color[] colors =
    {
        Color.red,
        Color.green,
        Color.blue,
        Color.yellow,
        new Color32(0,255,255,255),
        new Color32(165,42,42,255),
        new Color32(0,0,160,255),
        new Color32(0,255,0,255),
        new Color32(255,165,0,255),
        new Color32(192,192,192,255),
        new Color32(0,128,128,255)
    };
    void Start()
    {
        //=== FOR TEXT ===
        /*TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        int r = Random.Range(0, colors.Length);
        string tex = $"<color={colors[r]}>T.R.U.D.G.E</color>";
        text.text = tex;*/

        //=== IMAGE ===
        //Image image = GetComponent<Image>();
        InvokeRepeating(nameof(ChangeColor), 0f, 2f);
    }
    void ChangeColor()
    {
        int r = Random.Range(0,colors.Length);
        image.color = colors[r];
    }
}
