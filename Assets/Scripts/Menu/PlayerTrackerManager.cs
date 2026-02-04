using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlayerTrackerManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    public string scene = "Level1";
    private List<PlayerInput> playerInputs = new List<PlayerInput>();
    [SerializeField] private List<GameObject> players;
    private bool allPlayersSpawned = false;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    public void HandlePlayerJoined(PlayerInput obj)
    {
        if (!playerInputs.Contains(obj) && obj != null)
        {

            playerInputs.Add(obj);
            GameObject playerSpawned = obj.transform.root.gameObject;
            players.Add(playerSpawned);
            DontDestroyOnLoad(playerSpawned);

            UpdateAllPlayerCameras();
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "SelectionScreen" && SceneManager.GetActiveScene().name != "MainMenu" && !allPlayersSpawned)
        {
            allPlayersSpawned = true;
            PlayerInputManager.instance.DisableJoining();
            MovePlayersToSpawnPoints();
        }
        else if(SceneManager.GetActiveScene().name == "SelectionScreen" || SceneManager.GetActiveScene().name == "MainMenu")
        {
            allPlayersSpawned = false;

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                SceneManager.LoadSceneAsync(scene);
            }

            PlayerInputManager.instance.EnableJoining();
        }
    }

    void MovePlayersToSpawnPoints()
    {
        SpawnPointVisualizer[] spawns = GameObject.FindObjectsByType<SpawnPointVisualizer>(FindObjectsSortMode.None);

        for (int i = 0; i < playerInputs.Count; i++)
        {
            if (i < spawns.Length)
            {
                playerInputs[i].GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                playerInputs[i].transform.position = spawns[i].transform.position + Vector3.up * 3;
                playerInputs[i].transform.rotation = spawns[i].GetRotation();
                if(i == 0)
                {
                    playerInputs[i].GetComponent<PlayerCamera>().MinimapPrep();
                }
            }
        }

        foreach (var player in players)
        {
            player.SetActive(true);
        }
    }

    private void UpdateAllPlayerCameras()
    {
        int currentTotal = playerInputs.Count;
        foreach (var player in players)
        {
            var splitCam = player.GetComponentInChildren<SplitScreenCamera>();
            if (splitCam != null)
            {
                splitCam.SetupCamera(currentTotal);
            }
        }
    }
}
