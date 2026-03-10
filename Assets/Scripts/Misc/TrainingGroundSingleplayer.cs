using UnityEngine;

public class TrainingGroundSinglelapimage : MonoBehaviour
{
    void Start()
    {
        TrainingGroundReady[] tr = FindObjectsOfType<TrainingGroundReady>();
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
            //GameObject fef = players.Find("LapImage");
            /*GameObject training = GameObject.Find("TrainingUI");
            if (training != null)
            {
                training.SetActive(true);
            }
            GameObject ttext = GameObject.Find("TrainingText");
            if (ttext != null)
            {
                ttext.SetActive(false);
            }
            GameObject rtext = GameObject.Find("RadyText");
            if (rtext != null)
            {
                rtext.SetActive(false);
            }*/
        }
    }
}
