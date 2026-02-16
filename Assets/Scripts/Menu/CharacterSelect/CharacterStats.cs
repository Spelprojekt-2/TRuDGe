using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI characterName;

    [Header("P1")]
    public GameObject topSpeedSlider;
    public GameObject accelerationSlider;
    public GameObject turnSpeedSlider;

    [Header("P2")]
    public GameObject topSpeedSlider1;
    public GameObject accelerationSlider1;
    public GameObject turnSpeedSlider1;

    [Header("P3")]
    public GameObject topSpeedSlider2;
    public GameObject accelerationSlider2;
    public GameObject turnSpeedSlider2;

    [Header("P4")]
    public GameObject topSpeedSlider3;
    public GameObject accelerationSlider3;
    public GameObject turnSpeedSlider3;
 
    void Start()
    {

    }

    public void SwapCharacterStats(int playerIndex)
    {
        string selectedCharacter = characterName.text;
        if (playerIndex == 0)
        {
        if (selectedCharacter == "Lars-Göran")
        {
            topSpeedSlider.GetComponent<Slider>().value = 0.5f;
            accelerationSlider.GetComponent<Slider>().value = 0.5f;
            turnSpeedSlider.GetComponent<Slider>().value = 0.5f;
        }
        else if (selectedCharacter == "Nina Hagen")
        {
            topSpeedSlider.GetComponent<Slider>().value = 0.6f;
            accelerationSlider.GetComponent<Slider>().value = 0.1f;
            turnSpeedSlider.GetComponent<Slider>().value = 1f;
        }
        else if (selectedCharacter == "Carla Capô")
        {
            topSpeedSlider.GetComponent<Slider>().value = 1f;
            accelerationSlider.GetComponent<Slider>().value = 1f;
            turnSpeedSlider.GetComponent<Slider>().value = 1f;
        }
        else if (selectedCharacter == "Leonie Arenberg")
        {
            topSpeedSlider.GetComponent<Slider>().value = 0.9f;
            accelerationSlider.GetComponent<Slider>().value = 0.8f;
            turnSpeedSlider.GetComponent<Slider>().value = 0.2f;
        }
        else if (selectedCharacter == "King Napoleon VIII")
        {
            topSpeedSlider.GetComponent<Slider>().value = 0.1f;
            accelerationSlider.GetComponent<Slider>().value = 0.3f;
            turnSpeedSlider.GetComponent<Slider>().value = 0.7f;
        }
        else if (selectedCharacter == "André Nuskea")
        {
            topSpeedSlider.GetComponent<Slider>().value = 0.6f;
            accelerationSlider.GetComponent<Slider>().value = 0.4f;
            turnSpeedSlider.GetComponent<Slider>().value = 0.9f;
        }
        else if (selectedCharacter == "Ragana Vilkaite")
        {
            topSpeedSlider.GetComponent<Slider>().value = 0.4f;
            accelerationSlider.GetComponent<Slider>().value = 0.1f;
            turnSpeedSlider.GetComponent<Slider>().value = 0.5f;
        }
        else if (selectedCharacter == "Tristano Martinelli")
        {
            topSpeedSlider.GetComponent<Slider>().value = 0.1f;
            accelerationSlider.GetComponent<Slider>().value = 0.5f;
            turnSpeedSlider.GetComponent<Slider>().value = 0.2f;
        }
        }
        else if (playerIndex == 1)
        {
            if (selectedCharacter == "Lars-Göran")
        {
            topSpeedSlider1.GetComponent<Slider>().value = 0.5f;
            accelerationSlider1.GetComponent<Slider>().value = 0.5f;
            turnSpeedSlider1.GetComponent<Slider>().value = 0.5f;
        }
        else if (selectedCharacter == "Nina Hagen")
        {
            topSpeedSlider1.GetComponent<Slider>().value = 0.6f;
            accelerationSlider1.GetComponent<Slider>().value = 0.1f;
            turnSpeedSlider1.GetComponent<Slider>().value = 1f;
        }
        else if (selectedCharacter == "Carla Capô")
        {
            topSpeedSlider1.GetComponent<Slider>().value = 1f;
            accelerationSlider1.GetComponent<Slider>().value = 1f;
            turnSpeedSlider1.GetComponent<Slider>().value = 1f;
        }
        else if (selectedCharacter == "Leonie Arenberg")
        {
            topSpeedSlider1.GetComponent<Slider>().value = 0.9f;
            accelerationSlider1.GetComponent<Slider>().value = 0.8f;
            turnSpeedSlider1.GetComponent<Slider>().value = 0.2f;
        }
        else if (selectedCharacter == "King Napoleon VIII")
        {
            topSpeedSlider1.GetComponent<Slider>().value = 0.1f;
            accelerationSlider1.GetComponent<Slider>().value = 0.3f;
            turnSpeedSlider1.GetComponent<Slider>().value = 0.7f;
        }
        else if (selectedCharacter == "André Nuskea")
        {
            topSpeedSlider1.GetComponent<Slider>().value = 0.6f;
            accelerationSlider1.GetComponent<Slider>().value = 0.4f;
            turnSpeedSlider1.GetComponent<Slider>().value = 0.9f;
        }
        else if (selectedCharacter == "Ragana Vilkaite")
        {
            topSpeedSlider1.GetComponent<Slider>().value = 0.4f;
            accelerationSlider1.GetComponent<Slider>().value = 0.1f;
            turnSpeedSlider1.GetComponent<Slider>().value = 0.5f;
        }
        else if (selectedCharacter == "Tristano Martinelli")
        {
            topSpeedSlider1.GetComponent<Slider>().value = 0.1f;
            accelerationSlider1.GetComponent<Slider>().value = 0.5f;
            turnSpeedSlider1.GetComponent<Slider>().value = 0.2f;
        }
        }
        else if (playerIndex == 2)
        {
            if (selectedCharacter == "Lars-Göran")
        {
            topSpeedSlider2.GetComponent<Slider>().value = 0.5f;
            accelerationSlider2.GetComponent<Slider>().value = 0.5f;
            turnSpeedSlider2.GetComponent<Slider>().value = 0.5f;
        }
        else if (selectedCharacter == "Nina Hagen")
        {
            topSpeedSlider2.GetComponent<Slider>().value = 0.6f;
            accelerationSlider2.GetComponent<Slider>().value = 0.1f;
            turnSpeedSlider2.GetComponent<Slider>().value = 1f;
        }
        else if (selectedCharacter == "Carla Capô")
        {
            topSpeedSlider2.GetComponent<Slider>().value = 1f;
            accelerationSlider2.GetComponent<Slider>().value = 1f;
            turnSpeedSlider2.GetComponent<Slider>().value = 1f;
        }
        else if (selectedCharacter == "Leonie Arenberg")
        {
            topSpeedSlider2.GetComponent<Slider>().value = 0.9f;
            accelerationSlider2.GetComponent<Slider>().value = 0.8f;
            turnSpeedSlider2.GetComponent<Slider>().value = 0.2f;
        }
        else if (selectedCharacter == "King Napoleon VIII")
        {
            topSpeedSlider2.GetComponent<Slider>().value = 0.1f;
            accelerationSlider2.GetComponent<Slider>().value = 0.3f;
            turnSpeedSlider2.GetComponent<Slider>().value = 0.7f;
        }
        else if (selectedCharacter == "André Nuskea")
        {
            topSpeedSlider2.GetComponent<Slider>().value = 0.6f;
            accelerationSlider2.GetComponent<Slider>().value = 0.4f;
            turnSpeedSlider2.GetComponent<Slider>().value = 0.9f;
        }
        else if (selectedCharacter == "Ragana Vilkaite")
        {
            topSpeedSlider2.GetComponent<Slider>().value = 0.4f;
            accelerationSlider2.GetComponent<Slider>().value = 0.1f;
            turnSpeedSlider2.GetComponent<Slider>().value = 0.5f;
        }
        else if (selectedCharacter == "Tristano Martinelli")
        {
            topSpeedSlider2.GetComponent<Slider>().value = 0.1f;
            accelerationSlider2.GetComponent<Slider>().value = 0.5f;
            turnSpeedSlider2.GetComponent<Slider>().value = 0.2f;
        }
        }
        else if (playerIndex == 3)
        {
            if (selectedCharacter == "Lars-Göran")
        {
            topSpeedSlider3.GetComponent<Slider>().value = 0.5f;
            accelerationSlider3.GetComponent<Slider>().value = 0.5f;
            turnSpeedSlider3.GetComponent<Slider>().value = 0.5f;
        }
        else if (selectedCharacter == "Nina Hagen")
        {
            topSpeedSlider3.GetComponent<Slider>().value = 0.6f;
            accelerationSlider3.GetComponent<Slider>().value = 0.1f;
            turnSpeedSlider3.GetComponent<Slider>().value = 1f;
        }
        else if (selectedCharacter == "Carla Capô")
        {
            topSpeedSlider3.GetComponent<Slider>().value = 1f;
            accelerationSlider3.GetComponent<Slider>().value = 1f;
            turnSpeedSlider3.GetComponent<Slider>().value = 1f;
        }
        else if (selectedCharacter == "Leonie Arenberg")
        {
            topSpeedSlider3.GetComponent<Slider>().value = 0.9f;
            accelerationSlider3.GetComponent<Slider>().value = 0.8f;
            turnSpeedSlider3.GetComponent<Slider>().value = 0.2f;
        }
        else if (selectedCharacter == "King Napoleon VIII")
        {
            topSpeedSlider3.GetComponent<Slider>().value = 0.1f;
            accelerationSlider3.GetComponent<Slider>().value = 0.3f;
            turnSpeedSlider3.GetComponent<Slider>().value = 0.7f;
        }
        else if (selectedCharacter == "André Nuskea")
        {
            topSpeedSlider3.GetComponent<Slider>().value = 0.6f;
            accelerationSlider3.GetComponent<Slider>().value = 0.4f;
            turnSpeedSlider3.GetComponent<Slider>().value = 0.9f;
        }
        else if (selectedCharacter == "Ragana Vilkaite")
        {
            topSpeedSlider3.GetComponent<Slider>().value = 0.4f;
            accelerationSlider3.GetComponent<Slider>().value = 0.1f;
            turnSpeedSlider3.GetComponent<Slider>().value = 0.5f;
        }
        else if (selectedCharacter == "Tristano Martinelli")
        {
            topSpeedSlider3.GetComponent<Slider>().value = 0.1f;
            accelerationSlider3.GetComponent<Slider>().value = 0.5f;
            turnSpeedSlider3.GetComponent<Slider>().value = 0.2f;
        }
        }
    }
}
