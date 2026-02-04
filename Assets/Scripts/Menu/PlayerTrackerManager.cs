using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
public class PlayerTrackerManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    public string scene = "Level1";
    private List<PlayerInput> playerInputs = new List<PlayerInput>();
    [SerializeField] private List<GameObject> players;
    private bool allPlayersSpawned = false;

    private Dictionary<int, bool> readyStates = new();
    private SelectionUIList UIList;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        readyStates.Clear();
    }
    public void HandlePlayerJoined(PlayerInput obj)
    {
        if (!playerInputs.Contains(obj) && obj != null)
        {

            playerInputs.Add(obj);
            GameObject playerSpawned = obj.transform.root.gameObject;
            players.Add(playerSpawned);
            DontDestroyOnLoad(playerSpawned);
            readyStates[obj.playerIndex] = false;


            MovePlayersToSpawnPoints();
            UpdateAllPlayerCameras();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadmode)
    {
        if (SceneManager.GetActiveScene().name != "SelectionScreen" && SceneManager.GetActiveScene().name != "MainMenu" && !allPlayersSpawned)
        {
            allPlayersSpawned = true;
            PlayerInputManager.instance.DisableJoining();
            MovePlayersToSpawnPoints();
        }
        else if(SceneManager.GetActiveScene().name == "SelectionScreen" || SceneManager.GetActiveScene().name == "MainMenu")
        {
            UIList = FindFirstObjectByType<SelectionUIList>();
            allPlayersSpawned = false;
            PlayerInputManager.instance.EnableJoining();
        }
    }

    void MovePlayersToSpawnPoints()
    {
        SpawnPointVisualizer[] spawns = GameObject.FindObjectsByType<SpawnPointVisualizer>(FindObjectsSortMode.None).OrderBy(o => o.name).ToArray();

        for (int i = 0; i < playerInputs.Count; i++)
        {
            if (i < spawns.Length)
            {
                playerInputs[i].GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                playerInputs[i].transform.position = spawns[i].transform.position;
                playerInputs[i].transform.rotation = spawns[i].transform.rotation;
            }

            if (SceneManager.GetActiveScene().name != "SelectionScreen")
                players[i].GetComponentInChildren<PlayerInput>().SwitchCurrentActionMap("Player");
            else
                players[i].GetComponentInChildren<PlayerInput>().SwitchCurrentActionMap("UI");
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

    public void SetReady(PlayerInput input)
    {
        int player = input.playerIndex;
        readyStates[player] = true;
        switch (player)
        {
            case 0:
                UIList.ReadyTextP1.gameObject.SetActive(true);
                break;
            case 1:
                UIList.ReadyTextP2.gameObject.SetActive(true);
                break;
            case 2:
                UIList.ReadyTextP3.gameObject.SetActive(true);
                break;
            case 3:
                UIList.ReadyTextP4.gameObject.SetActive(true);
                break;
        }

        foreach (var isReady in readyStates.Values)
        {
            if (!isReady)
                return;
        }

        Debug.Log("Everyone is ready!");
        SceneManager.LoadScene(scene);
    }
    public void SetUnready(PlayerInput input)
    {
        int player = input.playerIndex;
        readyStates[player] = false;

        switch (player)
        {
            case 0:
                UIList.ReadyTextP1.gameObject.SetActive(false);
                break;
            case 1:
                UIList.ReadyTextP2.gameObject.SetActive(false);
                break;
            case 2:
                UIList.ReadyTextP3.gameObject.SetActive(false);
                break;
            case 3:
                UIList.ReadyTextP4.gameObject.SetActive(false);
                break;
        }
    }
}
