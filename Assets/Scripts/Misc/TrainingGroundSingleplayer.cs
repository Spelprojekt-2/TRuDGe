using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class TrainingGroundSingle : MonoBehaviour
{
    void Start()
    {
        /*GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject p in players)
            {
                Transform lapimage = p.transform.Find("Canvas/LapImage");
                if(lapimage != null)
                lapimage.GetComponent<Image>().enabled = false;

                Transform posimage = p.transform.Find("Canvas/RaceUI/PositionImage");
                if(posimage != null)
                posimage.GetComponent<Image>().enabled = false;

                Transform timer = p.transform.Find("Canvas/TimeTrialStuff/Timer");
                if(timer != null)
                timer.GetComponent<TextMeshProUGUI>().enabled = false;

                Transform KBMInputs = p.transform.Find("Canvas/TrainingUI/ControlsKBM");
                Transform ControllerInputs = p.transform.Find("Canvas/TrainingUI/ControlsController");
                PlayerInput input = p.GetComponentInChildren<PlayerInput>();
                bool isController = input.currentControlScheme == "Gamepad";
                Debug.Log(isController);
                if (isController)
                {
                    KBMInputs.GetComponent<TextMeshProUGUI>().enabled = false;
                    ControllerInputs.GetComponent<TextMeshProUGUI>().enabled = true;
                }
                else
                {
                    KBMInputs.GetComponent<TextMeshProUGUI>().enabled = true;
                    ControllerInputs.GetComponent<TextMeshProUGUI>().enabled = false;
                }
            }
           
        /*mapicon.GetComponent<MinimapIcons>().enabled = false;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            GameObject lapimage = GameObject.Find("LapImage");
            lapimage.GetComponent<Image>().enabled = false;

            GameObject posimage = GameObject.Find("PositionImage");
            posimage.GetComponent<Image>().enabled = false;
        }
        

        /*TrainingGroundReady[] tr = FindObjectsOfType<TrainingGroundReady>();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (TrainingGroundReady rdy in tr)
        {
            rdy.enabled = false;
            GameObject lapimage = GameObject.Find("LapImage");
            if (lapimage != null)
            {
                lapimage.SetActive(false);
            }
            GameObject raceui = GameObject.Find("RaceUI");
            if (raceui != null)
            {
                raceui.SetActive(false);
            }
            GameObject tts = GameObject.Find("TimeTrialStuff");
            if (tts != null)
            {
                tts.SetActive(false);
            }
            GameObject training = GameObject.Find("TrainingText");
            if (training != null)
            {
                training.SetActive(true);
            }
            GameObject tui = GameObject.Find("TrainingUI");
            if (tui != null)
            {
                tui.SetActive(false);
            }/*
            GameObject rtext = GameObject.Find("RadyText");
            if (rtext != null)
            {
                rtext.SetActive(false);
            }
        }*/
    }
}
