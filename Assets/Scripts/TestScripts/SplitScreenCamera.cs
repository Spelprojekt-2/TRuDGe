using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent (typeof(Camera))]
public class SplitScreenCamera : MonoBehaviour
{
    [SerializeField] PlayerInput input;
    private Camera cam;
    private int index;
    private int totalPlayers;

    private void Awake()
    {
        if(PlayerInputManager.instance != null) PlayerInputManager.instance.onPlayerJoined += HandlePlayerJoined;
    }

    private void HandlePlayerJoined(PlayerInput obj)
    {
        totalPlayers = PlayerInput.all.Count;
        if (totalPlayers >= 5) return;
        SetupCamera(totalPlayers);
    }

    private void Start()
    {
        index = input.playerIndex;
        totalPlayers = PlayerInput.all.Count;
        cam = GetComponent<Camera>();
        cam.depth = index;

        SetupCamera(totalPlayers);
    }

    public void SetupCamera(int totalPlayers)
    {
        if (cam == null) cam = GetComponent<Camera>();
        index = input.playerIndex;

        switch (totalPlayers)
        {
            case 1:
                cam.rect = new Rect(0, 0, 1, 1);
                break;
            case 2:
                cam.rect = new Rect(0, index == 1 ? 0 : 0.5f, 1f, 0.5f);
                break;
            case >= 3:
                cam.rect = new Rect(
                    (index % 2) * 0.5f,
                    (index < 2) ? 0.5f : 0,
                    0.5f,
                    0.5f);
                break;
        }
    }
}
