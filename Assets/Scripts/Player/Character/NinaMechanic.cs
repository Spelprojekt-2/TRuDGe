using UnityEngine;
using System.Collections;

public class NinaMechanic : MonoBehaviour
{
    [SerializeField] private ParticleSystem steam;
    [SerializeField] private Camera renderingCamera;
    [SerializeField] private bool specialEnabled;
    
    
    /*---- Ideas -----
    weld particles?
    guitar riff
    when hit wrenches fly out of the tank
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
        RacerData data = transform.root.GetComponentInChildren<RacerData>();
        
        if (!SceneController.instance.IsMenu)
        {
            if (data.racername == "The Brass Beast")
            {
                specialEnabled = true;
            }

            int layer;
            int playerIndex = data.index;
            switch (playerIndex)
            {
                case 0:
                    layer = LayerMask.NameToLayer("Player1fx");
                    steam.gameObject.layer = layer;
                    renderingCamera.cullingMask |= 1 << layer;
                    break;
                case 1: steam.gameObject.layer = LayerMask.NameToLayer("Player2fx"); 
                    layer = LayerMask.NameToLayer("Player2fx");
                    steam.gameObject.layer = layer;
                    renderingCamera.cullingMask |= 1 << layer;
                    break;
                case 2: steam.gameObject.layer = LayerMask.NameToLayer("Player3fx"); 
                    layer = LayerMask.NameToLayer("Player3fx");
                    steam.gameObject.layer = layer;
                    renderingCamera.cullingMask |= 1 << layer;
                    break;
                case 3: steam.gameObject.layer = LayerMask.NameToLayer("Player4fx"); 
                    layer = LayerMask.NameToLayer("Player4fx");
                    steam.gameObject.layer = layer;
                    renderingCamera.cullingMask |= 1 << layer;
                    break;
            }
        }
        if (SceneController.instance.IsMenu)
        {
            specialEnabled = false;
            steam.Stop();
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
     
    public IEnumerator LaunchWrenches()
    {
        steam.Play();
        yield return new WaitForSeconds(0.5f);
        steam.Stop();
    }
    void OnDisable()
    {
        SceneController.instance.SceneChangeEvent -= OnSceneLoaded;
    }
}
