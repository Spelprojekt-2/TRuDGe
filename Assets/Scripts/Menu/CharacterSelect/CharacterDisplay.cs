using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterDisplay : MonoBehaviour
{
    public GameObject characterDescription;

    [Header("P1")]
    public GameObject characterBackground;
    public GameObject characterText;
    public GameObject characterStats;

    [Header("P2")]
    public GameObject characterBackground1;
    public GameObject characterText1;
    public GameObject characterStats1;

    [Header("P3")]
    public GameObject characterBackground2;
    public GameObject characterText2;
    public GameObject characterStats2;

    [Header("P4")]
    public GameObject characterBackground3;
    public GameObject characterText3;
    public GameObject characterStats3;

    public void SetHighlight(bool state, int playerIndex)
    {
        if (playerIndex == 0)
    {
        characterBackground?.SetActive(state);
        characterText?.SetActive(state);
        characterStats?.SetActive(state);
    }
    else if (playerIndex == 1)
    {
        characterBackground1?.SetActive(state);
        characterText1?.SetActive(state);
        characterStats1?.SetActive(state);
    }
    else if (playerIndex == 2)
    {
        characterBackground2?.SetActive(state);
        characterText2?.SetActive(state);
        characterStats2?.SetActive(state);
    }
    else if (playerIndex == 3)
    {
        characterBackground3?.SetActive(state);
        characterText3?.SetActive(state);
        characterStats3?.SetActive(state);
    }
    
    }
}
