using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI characterName;

    [Header("P1")]
    public GameObject charismaSlider;
    public GameObject topSpeedSlider;
    public GameObject accelerationSlider;
    public GameObject turnSpeedSlider;

    [Header("P2")]
    public GameObject charismaSlider1;
    public GameObject topSpeedSlider1;
    public GameObject accelerationSlider1;
    public GameObject turnSpeedSlider1;

    [Header("P3")]
    public GameObject charismaSlider2;
    public GameObject topSpeedSlider2;
    public GameObject accelerationSlider2;
    public GameObject turnSpeedSlider2;

    [Header("P4")]
    public GameObject charismaSlider3;
    public GameObject topSpeedSlider3;
    public GameObject accelerationSlider3;
    public GameObject turnSpeedSlider3;

    public void SwapCharacterStats(int playerIndex)
    {
        if (SceneManager.GetActiveScene().name == "SelectionScreen")
        {
        string selectedCharacter = characterName.text;
        if (playerIndex == 0)
        {
        if (selectedCharacter == "Lars-Göran")
        {
            charismaSlider.GetComponent<Slider>().value = 0.5f;
            topSpeedSlider.GetComponent<Slider>().value = 0.5f;
            accelerationSlider.GetComponent<Slider>().value = 0.5f;
            turnSpeedSlider.GetComponent<Slider>().value = 0.5f;
        }
        else if (selectedCharacter == "Nina Hagen")
        {
            charismaSlider.GetComponent<Slider>().value = 0.8f;
            topSpeedSlider.GetComponent<Slider>().value = 0.6f;
            accelerationSlider.GetComponent<Slider>().value = 0.1f;
            turnSpeedSlider.GetComponent<Slider>().value = 1f;
        }
        else if (selectedCharacter == "Carla Capô")
        {
            charismaSlider.GetComponent<Slider>().value = 0.9f;
            topSpeedSlider.GetComponent<Slider>().value = 1f;
            accelerationSlider.GetComponent<Slider>().value = 1f;
            turnSpeedSlider.GetComponent<Slider>().value = 1f;
        }
        else if (selectedCharacter == "Leonie Arenberg")
        {
            charismaSlider.GetComponent<Slider>().value = 0.1f;
            topSpeedSlider.GetComponent<Slider>().value = 0.9f;
            accelerationSlider.GetComponent<Slider>().value = 0.8f;
            turnSpeedSlider.GetComponent<Slider>().value = 0.2f;
        }
        else if (selectedCharacter == "King Napoleon VIII")
        {
            charismaSlider.GetComponent<Slider>().value = 0f;
            topSpeedSlider.GetComponent<Slider>().value = 0.1f;
            accelerationSlider.GetComponent<Slider>().value = 0.3f;
            turnSpeedSlider.GetComponent<Slider>().value = 0.7f;
        }
        else if (selectedCharacter == "André Nuskea")
        {
            charismaSlider.GetComponent<Slider>().value = 0.1f;
            topSpeedSlider.GetComponent<Slider>().value = 0.6f;
            accelerationSlider.GetComponent<Slider>().value = 0.4f;
            turnSpeedSlider.GetComponent<Slider>().value = 0.9f;
        }
        else if (selectedCharacter == "Ragana Vilkaite")
        {
            charismaSlider.GetComponent<Slider>().value = 0.6f;
            topSpeedSlider.GetComponent<Slider>().value = 0.4f;
            accelerationSlider.GetComponent<Slider>().value = 0.1f;
            turnSpeedSlider.GetComponent<Slider>().value = 0.5f;
        }
        else if (selectedCharacter == "Tristano Martinelli")
        {
            charismaSlider.GetComponent<Slider>().value = 0.4f;
            topSpeedSlider.GetComponent<Slider>().value = 0.1f;
            accelerationSlider.GetComponent<Slider>().value = 0.5f;
            turnSpeedSlider.GetComponent<Slider>().value = 0.2f;
        }
        }
        else if (playerIndex == 1)
        {
            if (selectedCharacter == "Lars-Göran")
        {
            charismaSlider1.GetComponent<Slider>().value = 0.5f;
            topSpeedSlider1.GetComponent<Slider>().value = 0.5f;
            accelerationSlider1.GetComponent<Slider>().value = 0.5f;
            turnSpeedSlider1.GetComponent<Slider>().value = 0.5f;
        }
        else if (selectedCharacter == "Nina Hagen")
        {
            charismaSlider1.GetComponent<Slider>().value = 0.8f;
            topSpeedSlider1.GetComponent<Slider>().value = 0.6f;
            accelerationSlider1.GetComponent<Slider>().value = 0.1f;
            turnSpeedSlider1.GetComponent<Slider>().value = 1f;
        }
        else if (selectedCharacter == "Carla Capô")
        {
            charismaSlider1.GetComponent<Slider>().value = 0.9f;
            topSpeedSlider1.GetComponent<Slider>().value = 1f;
            accelerationSlider1.GetComponent<Slider>().value = 1f;
            turnSpeedSlider1.GetComponent<Slider>().value = 1f;
        }
        else if (selectedCharacter == "Leonie Arenberg")
        {
            charismaSlider1.GetComponent<Slider>().value = 0.1f;
            topSpeedSlider1.GetComponent<Slider>().value = 0.9f;
            accelerationSlider1.GetComponent<Slider>().value = 0.8f;
            turnSpeedSlider1.GetComponent<Slider>().value = 0.2f;
        }
        else if (selectedCharacter == "King Napoleon VIII")
        {
            charismaSlider1.GetComponent<Slider>().value = 0f;
            topSpeedSlider1.GetComponent<Slider>().value = 0.1f;
            accelerationSlider1.GetComponent<Slider>().value = 0.3f;
            turnSpeedSlider1.GetComponent<Slider>().value = 0.7f;
        }
        else if (selectedCharacter == "André Nuskea")
        {
            charismaSlider1.GetComponent<Slider>().value = 0.1f;
            topSpeedSlider1.GetComponent<Slider>().value = 0.6f;
            accelerationSlider1.GetComponent<Slider>().value = 0.4f;
            turnSpeedSlider1.GetComponent<Slider>().value = 0.9f;
        }
        else if (selectedCharacter == "Ragana Vilkaite")
        {
            charismaSlider1.GetComponent<Slider>().value = 0.6f;
            topSpeedSlider1.GetComponent<Slider>().value = 0.4f;
            accelerationSlider1.GetComponent<Slider>().value = 0.1f;
            turnSpeedSlider1.GetComponent<Slider>().value = 0.5f;
        }
        else if (selectedCharacter == "Tristano Martinelli")
        {
            charismaSlider1.GetComponent<Slider>().value = 0.4f;
            topSpeedSlider1.GetComponent<Slider>().value = 0.1f;
            accelerationSlider1.GetComponent<Slider>().value = 0.5f;
            turnSpeedSlider1.GetComponent<Slider>().value = 0.2f;
        }
        }
        else if (playerIndex == 2)
        {
            if (selectedCharacter == "Lars-Göran")
        {
            charismaSlider2.GetComponent<Slider>().value = 0.5f;
            topSpeedSlider2.GetComponent<Slider>().value = 0.5f;
            accelerationSlider2.GetComponent<Slider>().value = 0.5f;
            turnSpeedSlider2.GetComponent<Slider>().value = 0.5f;
        }
        else if (selectedCharacter == "Nina Hagen")
        {
            charismaSlider2.GetComponent<Slider>().value = 0.8f;
            topSpeedSlider2.GetComponent<Slider>().value = 0.6f;
            accelerationSlider2.GetComponent<Slider>().value = 0.1f;
            turnSpeedSlider2.GetComponent<Slider>().value = 1f;
        }
        else if (selectedCharacter == "Carla Capô")
        {
            charismaSlider2.GetComponent<Slider>().value = 0.9f;
            topSpeedSlider2.GetComponent<Slider>().value = 1f;
            accelerationSlider2.GetComponent<Slider>().value = 1f;
            turnSpeedSlider2.GetComponent<Slider>().value = 1f;
        }
        else if (selectedCharacter == "Leonie Arenberg")
        {
            charismaSlider2.GetComponent<Slider>().value = 0.1f;
            topSpeedSlider2.GetComponent<Slider>().value = 0.9f;
            accelerationSlider2.GetComponent<Slider>().value = 0.8f;
            turnSpeedSlider2.GetComponent<Slider>().value = 0.2f;
        }
        else if (selectedCharacter == "King Napoleon VIII")
        {
            charismaSlider2.GetComponent<Slider>().value = 0f;
            topSpeedSlider2.GetComponent<Slider>().value = 0.1f;
            accelerationSlider2.GetComponent<Slider>().value = 0.3f;
            turnSpeedSlider2.GetComponent<Slider>().value = 0.7f;
        }
        else if (selectedCharacter == "André Nuskea")
        {
            charismaSlider2.GetComponent<Slider>().value = 0.1f;
            topSpeedSlider2.GetComponent<Slider>().value = 0.6f;
            accelerationSlider2.GetComponent<Slider>().value = 0.4f;
            turnSpeedSlider2.GetComponent<Slider>().value = 0.9f;
        }
        else if (selectedCharacter == "Ragana Vilkaite")
        {
            charismaSlider2.GetComponent<Slider>().value = 0.6f;
            topSpeedSlider2.GetComponent<Slider>().value = 0.4f;
            accelerationSlider2.GetComponent<Slider>().value = 0.1f;
            turnSpeedSlider2.GetComponent<Slider>().value = 0.5f;
        }
        else if (selectedCharacter == "Tristano Martinelli")
        {
            charismaSlider2.GetComponent<Slider>().value = 0.4f;
            topSpeedSlider2.GetComponent<Slider>().value = 0.1f;
            accelerationSlider2.GetComponent<Slider>().value = 0.5f;
            turnSpeedSlider2.GetComponent<Slider>().value = 0.2f;
        }
        }
        else if (playerIndex == 3)
        {
            if (selectedCharacter == "Lars-Göran")
        {
            charismaSlider3.GetComponent<Slider>().value = 0.5f;
            topSpeedSlider3.GetComponent<Slider>().value = 0.5f;
            accelerationSlider3.GetComponent<Slider>().value = 0.5f;
            turnSpeedSlider3.GetComponent<Slider>().value = 0.5f;
        }
        else if (selectedCharacter == "Nina Hagen")
        {
            charismaSlider3.GetComponent<Slider>().value = 0.8f;
            topSpeedSlider3.GetComponent<Slider>().value = 0.6f;
            accelerationSlider3.GetComponent<Slider>().value = 0.1f;
            turnSpeedSlider3.GetComponent<Slider>().value = 1f;
        }
        else if (selectedCharacter == "Carla Capô")
        {
            charismaSlider3.GetComponent<Slider>().value = 0.9f;
            topSpeedSlider3.GetComponent<Slider>().value = 1f;
            accelerationSlider3.GetComponent<Slider>().value = 1f;
            turnSpeedSlider3.GetComponent<Slider>().value = 1f;
        }
        else if (selectedCharacter == "Leonie Arenberg")
        {
            charismaSlider3.GetComponent<Slider>().value = 0.1f;
            topSpeedSlider3.GetComponent<Slider>().value = 0.9f;
            accelerationSlider3.GetComponent<Slider>().value = 0.8f;
            turnSpeedSlider3.GetComponent<Slider>().value = 0.2f;
        }
        else if (selectedCharacter == "King Napoleon VIII")
        {
            charismaSlider3.GetComponent<Slider>().value = 0f;
            topSpeedSlider3.GetComponent<Slider>().value = 0.1f;
            accelerationSlider3.GetComponent<Slider>().value = 0.3f;
            turnSpeedSlider3.GetComponent<Slider>().value = 0.7f;
        }
        else if (selectedCharacter == "André Nuskea")
        {
            charismaSlider3.GetComponent<Slider>().value = 0.1f;
            topSpeedSlider3.GetComponent<Slider>().value = 0.6f;
            accelerationSlider3.GetComponent<Slider>().value = 0.4f;
            turnSpeedSlider3.GetComponent<Slider>().value = 0.9f;
        }
        else if (selectedCharacter == "Ragana Vilkaite")
        {
            charismaSlider3.GetComponent<Slider>().value = 0.6f;
            topSpeedSlider3.GetComponent<Slider>().value = 0.4f;
            accelerationSlider3.GetComponent<Slider>().value = 0.1f;
            turnSpeedSlider3.GetComponent<Slider>().value = 0.5f;
        }
        else if (selectedCharacter == "Tristano Martinelli")
        {
            charismaSlider3.GetComponent<Slider>().value = 0.4f;
            topSpeedSlider3.GetComponent<Slider>().value = 0.1f;
            accelerationSlider3.GetComponent<Slider>().value = 0.5f;
            turnSpeedSlider3.GetComponent<Slider>().value = 0.2f;
        }
        }
        }
    }
}
