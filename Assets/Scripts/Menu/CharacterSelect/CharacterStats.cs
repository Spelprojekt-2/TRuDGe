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
    public GameObject maxGasSlider;
    public GameObject shotCooldownSlider;

    [Header("P2")]
    public GameObject charismaSlider1;
    public GameObject topSpeedSlider1;
    public GameObject accelerationSlider1;
    public GameObject maxGasSlider1;
    public GameObject shotCooldownSlider1;

    [Header("P3")]
    public GameObject charismaSlider2;
    public GameObject topSpeedSlider2;
    public GameObject accelerationSlider2;
    public GameObject maxGasSlider2;
    public GameObject shotCooldownSlider2;

    [Header("P4")]
    public GameObject charismaSlider3;
    public GameObject topSpeedSlider3;
    public GameObject accelerationSlider3;
    public GameObject maxGasSlider3;
    public GameObject shotCooldownSlider3;

    public void SwapCharacterStats(int playerIndex)
    {
        if (SceneManager.GetActiveScene().name == "SelectionScreen" || SceneManager.GetActiveScene().name == "TimeTrialMenu")
        {
        string selectedCharacter = characterName.text;
        if (playerIndex == 0)
        {
        if (selectedCharacter == "Lars-Göran")
{
    charismaSlider.GetComponent<Slider>().value = 0.5f;
    topSpeedSlider.GetComponent<Slider>().value = 0.5f;
    accelerationSlider.GetComponent<Slider>().value = 0.5f;
    maxGasSlider.GetComponent<Slider>().value = 0.5f;
    shotCooldownSlider.GetComponent<Slider>().value = 0.5f;
}
else if (selectedCharacter == "The Brass Beast")
{
    charismaSlider.GetComponent<Slider>().value = 0.5f;
    topSpeedSlider.GetComponent<Slider>().value = 0.5f;
    accelerationSlider.GetComponent<Slider>().value = 0.5f;
    maxGasSlider.GetComponent<Slider>().value = 0.5f;
    shotCooldownSlider.GetComponent<Slider>().value = 0.5f;
}
else if (selectedCharacter == "Capôw")
{
    charismaSlider.GetComponent<Slider>().value = 0.5f;
    topSpeedSlider.GetComponent<Slider>().value = 0.5f;
    accelerationSlider.GetComponent<Slider>().value = 0.5f;
    maxGasSlider.GetComponent<Slider>().value = 0.5f;
    shotCooldownSlider.GetComponent<Slider>().value = 0.5f;
}
else if (selectedCharacter == "Schlammer")
{
    charismaSlider.GetComponent<Slider>().value = 0.5f;
    topSpeedSlider.GetComponent<Slider>().value = 0.5f;
    accelerationSlider.GetComponent<Slider>().value = 0.5f;
    maxGasSlider.GetComponent<Slider>().value = 0.5f;
    shotCooldownSlider.GetComponent<Slider>().value = 0.5f;
}
else if (selectedCharacter == "King Napoleon III")
{
    charismaSlider.GetComponent<Slider>().value = 0.5f;
    topSpeedSlider.GetComponent<Slider>().value = 0.5f;
    accelerationSlider.GetComponent<Slider>().value = 0.5f;
    maxGasSlider.GetComponent<Slider>().value = 0.5f;
    shotCooldownSlider.GetComponent<Slider>().value = 0.5f;
}
else if (selectedCharacter == "Dragoș")
{
    charismaSlider.GetComponent<Slider>().value = 0.5f;
    topSpeedSlider.GetComponent<Slider>().value = 0.5f;
    accelerationSlider.GetComponent<Slider>().value = 0.5f;
    maxGasSlider.GetComponent<Slider>().value = 0.5f;
    shotCooldownSlider.GetComponent<Slider>().value = 0.5f;
}
else if (selectedCharacter == "Demon of Vilkmergéle")
{
    charismaSlider.GetComponent<Slider>().value = 0.5f;
    topSpeedSlider.GetComponent<Slider>().value = 0.5f;
    accelerationSlider.GetComponent<Slider>().value = 0.5f;
    maxGasSlider.GetComponent<Slider>().value = 0.5f;
    shotCooldownSlider.GetComponent<Slider>().value = 0.5f;
}
else if (selectedCharacter == "Harlequini Martinellini")
{
    charismaSlider.GetComponent<Slider>().value = 0.5f;
    topSpeedSlider.GetComponent<Slider>().value = 0.5f;
    accelerationSlider.GetComponent<Slider>().value = 0.5f;
    maxGasSlider.GetComponent<Slider>().value = 0.5f;
    shotCooldownSlider.GetComponent<Slider>().value = 0.5f;
}
}
else if (playerIndex == 1)
{
    if (selectedCharacter == "Lars-Göran")
    {
        charismaSlider1.GetComponent<Slider>().value = 0.5f;
        topSpeedSlider1.GetComponent<Slider>().value = 0.5f;
        accelerationSlider1.GetComponent<Slider>().value = 0.5f;
        maxGasSlider1.GetComponent<Slider>().value = 0.5f;
        shotCooldownSlider1.GetComponent<Slider>().value = 0.5f;
    }
    else if (selectedCharacter == "The Brass Beast")
    {
        charismaSlider1.GetComponent<Slider>().value = 0.5f;
        topSpeedSlider1.GetComponent<Slider>().value = 0.5f;
        accelerationSlider1.GetComponent<Slider>().value = 0.5f;
        maxGasSlider1.GetComponent<Slider>().value = 0.5f;
        shotCooldownSlider1.GetComponent<Slider>().value = 0.5f;
    }
    else if (selectedCharacter == "Capôw")
    {
        charismaSlider1.GetComponent<Slider>().value = 0.5f;
        topSpeedSlider1.GetComponent<Slider>().value = 0.5f;
        accelerationSlider1.GetComponent<Slider>().value = 0.5f;
        maxGasSlider1.GetComponent<Slider>().value = 0.5f;
        shotCooldownSlider1.GetComponent<Slider>().value = 0.5f;
    }
    else if (selectedCharacter == "Schlammer")
    {
        charismaSlider1.GetComponent<Slider>().value = 0.5f;
        topSpeedSlider1.GetComponent<Slider>().value = 0.5f;
        accelerationSlider1.GetComponent<Slider>().value = 0.5f;
        maxGasSlider1.GetComponent<Slider>().value = 0.5f;
        shotCooldownSlider1.GetComponent<Slider>().value = 0.5f;
    }
    else if (selectedCharacter == "King Napoleon III")
    {
        charismaSlider1.GetComponent<Slider>().value = 0.5f;
        topSpeedSlider1.GetComponent<Slider>().value = 0.5f;
        accelerationSlider1.GetComponent<Slider>().value = 0.5f;
        maxGasSlider1.GetComponent<Slider>().value = 0.5f;
        shotCooldownSlider1.GetComponent<Slider>().value = 0.5f;
    }
    else if (selectedCharacter == "Dragoș")
    {
        charismaSlider1.GetComponent<Slider>().value = 0.5f;
        topSpeedSlider1.GetComponent<Slider>().value = 0.5f;
        accelerationSlider1.GetComponent<Slider>().value = 0.5f;
        maxGasSlider1.GetComponent<Slider>().value = 0.5f;
        shotCooldownSlider1.GetComponent<Slider>().value = 0.5f;
    }
    else if (selectedCharacter == "Demon of Vilkmergéle")
    {
        charismaSlider1.GetComponent<Slider>().value = 0.5f;
        topSpeedSlider1.GetComponent<Slider>().value = 0.5f;
        accelerationSlider1.GetComponent<Slider>().value = 0.5f;
        maxGasSlider1.GetComponent<Slider>().value = 0.5f;
        shotCooldownSlider1.GetComponent<Slider>().value = 0.5f;
    }
    else if (selectedCharacter == "Harlequini Martinellini")
    {
        charismaSlider1.GetComponent<Slider>().value = 0.5f;
        topSpeedSlider1.GetComponent<Slider>().value = 0.5f;
        accelerationSlider1.GetComponent<Slider>().value = 0.5f;
        maxGasSlider1.GetComponent<Slider>().value = 0.5f;
        shotCooldownSlider1.GetComponent<Slider>().value = 0.5f;
    }
        }
        else if (playerIndex == 2)
        {
            if (selectedCharacter == "Lars-Göran")
{
    charismaSlider2.GetComponent<Slider>().value = 0.5f;
    topSpeedSlider2.GetComponent<Slider>().value = 0.5f;
    accelerationSlider2.GetComponent<Slider>().value = 0.5f;
    maxGasSlider2.GetComponent<Slider>().value = 0.5f;
    shotCooldownSlider2.GetComponent<Slider>().value = 0.5f;
}
else if (selectedCharacter == "The Brass Beast")
{
    charismaSlider2.GetComponent<Slider>().value = 0.5f;
    topSpeedSlider2.GetComponent<Slider>().value = 0.5f;
    accelerationSlider2.GetComponent<Slider>().value = 0.5f;
    maxGasSlider2.GetComponent<Slider>().value = 0.5f;
    shotCooldownSlider2.GetComponent<Slider>().value = 0.5f;
}
else if (selectedCharacter == "Capôw")
{
    charismaSlider2.GetComponent<Slider>().value = 0.5f;
    topSpeedSlider2.GetComponent<Slider>().value = 0.5f;
    accelerationSlider2.GetComponent<Slider>().value = 0.5f;
    maxGasSlider2.GetComponent<Slider>().value = 0.5f;
    shotCooldownSlider2.GetComponent<Slider>().value = 0.5f;
}
else if (selectedCharacter == "Schlammer")
{
    charismaSlider2.GetComponent<Slider>().value = 0.5f;
    topSpeedSlider2.GetComponent<Slider>().value = 0.5f;
    accelerationSlider2.GetComponent<Slider>().value = 0.5f;
    maxGasSlider2.GetComponent<Slider>().value = 0.5f;
    shotCooldownSlider2.GetComponent<Slider>().value = 0.5f;
}
else if (selectedCharacter == "King Napoleon III")
{
    charismaSlider2.GetComponent<Slider>().value = 0.5f;
    topSpeedSlider2.GetComponent<Slider>().value = 0.5f;
    accelerationSlider2.GetComponent<Slider>().value = 0.5f;
    maxGasSlider2.GetComponent<Slider>().value = 0.5f;
    shotCooldownSlider2.GetComponent<Slider>().value = 0.5f;
}
else if (selectedCharacter == "Dragoș")
{
    charismaSlider2.GetComponent<Slider>().value = 0.5f;
    topSpeedSlider2.GetComponent<Slider>().value = 0.5f;
    accelerationSlider2.GetComponent<Slider>().value = 0.5f;
    maxGasSlider2.GetComponent<Slider>().value = 0.5f;
    shotCooldownSlider2.GetComponent<Slider>().value = 0.5f;
}
else if (selectedCharacter == "Demon of Vilkmergéle")
{
    charismaSlider2.GetComponent<Slider>().value = 0.5f;
    topSpeedSlider2.GetComponent<Slider>().value = 0.5f;
    accelerationSlider2.GetComponent<Slider>().value = 0.5f;
    maxGasSlider2.GetComponent<Slider>().value = 0.5f;
    shotCooldownSlider2.GetComponent<Slider>().value = 0.5f;
}
else if (selectedCharacter == "Harlequini Martinellini")
{
    charismaSlider2.GetComponent<Slider>().value = 0.5f;
    topSpeedSlider2.GetComponent<Slider>().value = 0.5f;
    accelerationSlider2.GetComponent<Slider>().value = 0.5f;
    maxGasSlider2.GetComponent<Slider>().value = 0.5f;
    shotCooldownSlider2.GetComponent<Slider>().value = 0.5f;
}
}
else if (playerIndex == 3)
{
    if (selectedCharacter == "Lars-Göran")
    {
        charismaSlider3.GetComponent<Slider>().value = 0.5f;
        topSpeedSlider3.GetComponent<Slider>().value = 0.5f;
        accelerationSlider3.GetComponent<Slider>().value = 0.5f;
        maxGasSlider3.GetComponent<Slider>().value = 0.5f;
        shotCooldownSlider3.GetComponent<Slider>().value = 0.5f;
    }
    else if (selectedCharacter == "The Brass Beast")
    {
        charismaSlider3.GetComponent<Slider>().value = 0.5f;
        topSpeedSlider3.GetComponent<Slider>().value = 0.5f;
        accelerationSlider3.GetComponent<Slider>().value = 0.5f;
        maxGasSlider3.GetComponent<Slider>().value = 0.5f;
        shotCooldownSlider3.GetComponent<Slider>().value = 0.5f;
    }
    else if (selectedCharacter == "Capôw")
    {
        charismaSlider3.GetComponent<Slider>().value = 0.5f;
        topSpeedSlider3.GetComponent<Slider>().value = 0.5f;
        accelerationSlider3.GetComponent<Slider>().value = 0.5f;
        maxGasSlider3.GetComponent<Slider>().value = 0.5f;
        shotCooldownSlider3.GetComponent<Slider>().value = 0.5f;
    }
    else if (selectedCharacter == "Schlammer")
    {
        charismaSlider3.GetComponent<Slider>().value = 0.5f;
        topSpeedSlider3.GetComponent<Slider>().value = 0.5f;
        accelerationSlider3.GetComponent<Slider>().value = 0.5f;
        maxGasSlider3.GetComponent<Slider>().value = 0.5f;
        shotCooldownSlider3.GetComponent<Slider>().value = 0.5f;
    }
    else if (selectedCharacter == "King Napoleon III")
    {
        charismaSlider3.GetComponent<Slider>().value = 0.5f;
        topSpeedSlider3.GetComponent<Slider>().value = 0.5f;
        accelerationSlider3.GetComponent<Slider>().value = 0.5f;
        maxGasSlider3.GetComponent<Slider>().value = 0.5f;
        shotCooldownSlider3.GetComponent<Slider>().value = 0.5f;
    }
    else if (selectedCharacter == "Dragoș")
    {
        charismaSlider3.GetComponent<Slider>().value = 0.5f;
        topSpeedSlider3.GetComponent<Slider>().value = 0.5f;
        accelerationSlider3.GetComponent<Slider>().value = 0.5f;
        maxGasSlider3.GetComponent<Slider>().value = 0.5f;
        shotCooldownSlider3.GetComponent<Slider>().value = 0.5f;
    }
    else if (selectedCharacter == "Demon of Vilkmergéle")
    {
        charismaSlider3.GetComponent<Slider>().value = 0.5f;
        topSpeedSlider3.GetComponent<Slider>().value = 0.5f;
        accelerationSlider3.GetComponent<Slider>().value = 0.5f;
        maxGasSlider3.GetComponent<Slider>().value = 0.5f;
        shotCooldownSlider3.GetComponent<Slider>().value = 0.5f;
    }
    else if (selectedCharacter == "Harlequini Martinellini")
    {
        charismaSlider3.GetComponent<Slider>().value = 0.5f;
        topSpeedSlider3.GetComponent<Slider>().value = 0.5f;
        accelerationSlider3.GetComponent<Slider>().value = 0.5f;
        maxGasSlider3.GetComponent<Slider>().value = 0.5f;
        shotCooldownSlider3.GetComponent<Slider>().value = 0.5f;
    }
    }
    }
    }
}
