using UnityEngine;

public class CharacterSpecials : MonoBehaviour
{
    [SerializeField] private LarsMessage lm;
    [SerializeField] private NinaMechanic nm;
    [SerializeField] private NapoleonRespect nr;
    [SerializeField] private ShudderChat sc;
    [SerializeField] private RacerData rd;
    private bool specialsEnabled = true;
    public void ToggleCharacterSpecials()
    {
        if (specialsEnabled)
        {
            switch (rd.racername)
            {
                case "Lars-Göran": lm.toggleMessage(false); break;
                case "The Brass Beast": nm.toggleWrenches(false); break;
                case "King Napoleon III": nr.toggleRespect(false); break;
                case "Capôw": sc.EnableChat(false); break;
            }
            specialsEnabled = false;
        }
        else
        {
            switch (rd.racername)
            {
                case "Lars-Göran": lm.toggleMessage(true); break;
                case "The Brass Beast": nm.toggleWrenches(true); break;
                case "King Napoleon III": nr.toggleRespect(true); break;
                case "Capôw": sc.EnableChat(true); break;
            }
            specialsEnabled = true;
        }
    }
}
