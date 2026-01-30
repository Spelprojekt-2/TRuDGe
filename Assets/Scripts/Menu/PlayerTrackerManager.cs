using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlayerTrackerManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    private List<PlayerInput> playerInputs = new List<PlayerInput>();
    [SerializeField] private List<GameObject> players = new List<GameObject>();
    private bool allPlayersSpawned = false;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void HandlePlayerJoined(PlayerInput obj)
    {
        Debug.Log("Here");
        if (!playerInputs.Contains(obj))
        {
            playerInputs.Add(obj);
            players.Add(obj.GetComponent<GameObject>());
            foreach (var player in players)
            {
                player.SetActive(false);
            }
            DontDestroyOnLoad(obj.gameObject);
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
        else
        {
            allPlayersSpawned = false;
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
            player.SetActive(false);
        }
    }
}
