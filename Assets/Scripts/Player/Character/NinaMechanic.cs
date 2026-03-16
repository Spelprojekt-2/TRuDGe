using UnityEngine;
using System.Collections;

public class NinaMechanic : MonoBehaviour
{
    [SerializeField] private ParticleSystem steam;
    [SerializeField] private bool specialEnabled;
    
    
    /*---- Ideas -----
    weld particles?
    guitar riff
    */
    void Awake()
    {
        steam.Stop();
    }
    void Start()
    {
        SceneController.instance.SceneChangeEvent += OnSceneLoaded;
    }
    void OnSceneLoaded()
    {
        if (!SceneController.instance.IsMenu)
        {
            RacerData data = transform.root.GetComponentInChildren<RacerData>();
            if (data.racername == "The Brass Beast")
            {
                specialEnabled = true;
            }
        }
        if (SceneController.instance.IsMenu)
        {
            specialEnabled = false;
        }
    }
    public void VentSteam()
    {
        if (steam.isPlaying == false && specialEnabled)
        {
            steam.Play();
            /*yield return new WaitForSeconds(2f);
            steam.Stop();*/
        }
        else
        {
            steam.Stop();
        }
    }
    void OnDisable()
    {
        SceneController.instance.SceneChangeEvent -= OnSceneLoaded;
    }
}
