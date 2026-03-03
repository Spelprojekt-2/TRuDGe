using UnityEngine;
using FMODUnity;

[CreateAssetMenu(menuName = "Scriptables/Audio/UI")]
public class UIAudio : ScriptableObject
{
    [SerializeField] private EventReference ui_btn_ref;
    [SerializeField] private EventReference ui_btn_special_ref;

    public void PlayUIBtnAudio()
    {
        if (ui_btn_ref.IsNull)
        {
            Debug.LogWarning("ui_button reference is missing!");
            return;
        }
        RuntimeManager.PlayOneShot(ui_btn_ref);
    }

    public void PlayUISpecialBtnAudio()
    {
        if (ui_btn_special_ref.IsNull)
        {
            Debug.LogWarning("ui_special_button reference is missing!");
            return;
        }
        RuntimeManager.PlayOneShot(ui_btn_special_ref);
    }
}
