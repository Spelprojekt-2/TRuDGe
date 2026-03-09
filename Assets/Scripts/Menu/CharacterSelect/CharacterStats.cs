using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI characterName;

    [Header("Stats")]
    public GameObject[] charismaSlider;
    public GameObject[] topSpeedSlider;
    public GameObject[] accelerationSlider;
    public GameObject[] maxGasSlider;
    public GameObject[] shotCooldownSlider;

    public void SwapCharacterStats(int playerIndex)
    {
        if (SceneManager.GetActiveScene().name == "SelectionScreen" || SceneManager.GetActiveScene().name == "TimeTrialMenu")
        {
            string selectedCharacter = characterName.text;
            if (selectedCharacter == "Lars-Göran")
            {
                charismaSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
                topSpeedSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
                accelerationSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
                maxGasSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
                shotCooldownSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
            }
            else if (selectedCharacter == "The Brass Beast")
            {
                charismaSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
                topSpeedSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
                accelerationSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
                maxGasSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
                shotCooldownSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
            }
            else if (selectedCharacter == "Capôw")
            {
                charismaSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
                topSpeedSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
                accelerationSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
                maxGasSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
                shotCooldownSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
            }
            else if (selectedCharacter == "Schlammer")
            {
                charismaSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
                topSpeedSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
                accelerationSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
                maxGasSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
                shotCooldownSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
            }
            else if (selectedCharacter == "King Napoleon III")
            {
                charismaSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
                topSpeedSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
                accelerationSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
                maxGasSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
                shotCooldownSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
            }
            else if (selectedCharacter == "Dragoș")
            {
                charismaSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
                topSpeedSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
                accelerationSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
                maxGasSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
                shotCooldownSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
            }
            else if (selectedCharacter == "Demon of Vilkmergéle")
            {
                charismaSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
                topSpeedSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
                accelerationSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
                maxGasSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
                shotCooldownSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
            }
            else if (selectedCharacter == "Harlequini Martinellini")
            {
                charismaSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
                topSpeedSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
                accelerationSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
                maxGasSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
                shotCooldownSlider[playerIndex].GetComponent<Slider>().value = 0.5f;
            }
        }
    }
}
