using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlayerTrackerManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
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
                SceneManager.LoadSceneAsync("LeoTest");
            }

            PlayerInputManager.instance.EnableJoining();
        }
    }

    void MovePlayersToSpawnPoints()
    {
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("SpawnPoint");

        for (int i = 0; i < playerInputs.Count; i++)
        {
            if (i < spawns.Length)
            {
                playerInputs[i].transform.position = spawns[i].transform.position;
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
