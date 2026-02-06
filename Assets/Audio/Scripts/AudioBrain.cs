using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(StudioListener))]
public class PlayerAudioBrain : MonoBehaviour
{
    private StudioListener _studioListener;
    [SerializeField] private GameObject _attenuationObject;

    void Awake()
    {
        // Get FMOD listener ref
        _studioListener = GetComponent<StudioListener>();
    }

    void Start()
    {
        // Get attenuationObject as playerOBJ
        if (_attenuationObject == null)
        {
            Debug.LogWarning("AudioBrain: AttenuationObject is not set. Uses obj with player tag instead.");
            GameObject _playerOBJ = GameObject.FindWithTag("Player");
            if (!_playerOBJ)
            {
                Debug.LogError("AudioBrain: PlayerOBJ not found!");
                this.enabled = false;
                return;
            }
            _attenuationObject = _playerOBJ;
        }

        // Set listener-attenuationObject
        _studioListener.AttenuationObject = _attenuationObject;
    }
}
