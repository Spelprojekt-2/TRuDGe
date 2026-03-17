using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;
using TMPro;

[RequireComponent(typeof(Slider))]
public class FMODVCASlider : MonoBehaviour
{
    [Header("FMOD VCA Path")]
    [SerializeField] private string vcaPath = "vca:/MASTER";

    [Header("UI")]
    [SerializeField] private TMP_Text text_percentage;
    private Slider volumeSlider;

    private VCA vca;

    void Start()
    {
        // Get slider
        volumeSlider = GetComponent<Slider>();

        // Get the VCA reference
        vca = RuntimeManager.GetVCA(vcaPath);

        // Initialize slider with current VCA volume
        float currentVolume;
        vca.getVolume(out currentVolume);
        volumeSlider.value = currentVolume;

        // Listen to slider changes
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float value)
    {
        vca.setVolume(value);

        // Update percentage text
        if (text_percentage != null)
        {
            text_percentage.text = Mathf.Round(value * 100f).ToString() + "%";
        }
    }
}