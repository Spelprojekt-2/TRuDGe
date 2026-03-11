using UnityEngine;

public class CharAfterRace : MonoBehaviour
{
    public GameObject[] characters;

    public void Display2DCharacter(string racen)
    {
        switch (racen)
        {
            case "Lars-Göran": characters[0].SetActive(true); break;
            case "The Brass Beast": characters[1].SetActive(true); break;
            case "Capôw": characters[2].SetActive(true); break;
            case "Schlammer": characters[3].SetActive(true); break;
            case "King Napoleon III": characters[4].SetActive(true); break;
            case "Dragoș": characters[5].SetActive(true); break;
            case "Demon of Vilkmergéle": characters[6].SetActive(true); break;
            case "Harlequini Martinellini": characters[7].SetActive(true); break;
        }
    }
}
