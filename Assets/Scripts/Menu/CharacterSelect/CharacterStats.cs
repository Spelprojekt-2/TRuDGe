using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class CharacterStats : MonoBehaviour
{
    [Header("Sliders")]
    public GameObject topSpeedSlider;
    public GameObject accelerationSlider;
    public GameObject turnSpeedSlider;
    [SerializeField] private TextMeshProUGUI characterName;
 
    void Start()
    {
        SwapCharacterStats();
    }

    public void SwapCharacterStats()
    {
        string selectedCharacter = characterName.text;

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
}
