using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.InputSystem.Users;
public class PlayerTrackerManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    private Dictionary<int, PlayerInput> playerInputs = new();
    private bool allPlayersSpawned = false;
    private bool isMenu = true;

    private Dictionary<int, bool> readyStates = new();
    private SelectionUIList UIList;
    private RacerData[] leaderboard;
    private void Awake()
    {
        if (FindObjectsByType<PlayerTrackerManager>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }
    public void HandlePlayerJoined(PlayerInput input)
    {
        if (!input)
            return;

        int index = input.playerIndex;

        if (playerInputs.ContainsKey(index))
            return;

        playerInputs[index] = input;
        readyStates[index] = false;

        DontDestroyOnLoad(input.transform.root.gameObject);

        MovePlayersToSpawnPoints();
        UpdateAllPlayerCameras();

        input.SwitchCurrentActionMap("UI");
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            FindAnyObjectByType<MainMenuUIController>().HideJoinPopup();
            PlayerInputManager.instance.DisableJoining();
        }
    }

    public void HandlePlayerLeft(PlayerInput leavingInput)
    {
        if (leavingInput == null)
            return;

        int leavingIndex = leavingInput.playerIndex;
        var higherIndices = playerInputs.Keys
            .Where(k => k > leavingIndex)
            .OrderBy(k => k)
            .ToList();

        // Simulate player leaving by shifting inputs per player
        foreach (int index in higherIndices)
        {
            PlayerInput nextPlayer = playerInputs[index];
            foreach (var device in nextPlayer.devices)
            {
                nextPlayer.SwitchCurrentControlScheme(device.displayName); // optional: update scheme
                leavingInput.SwitchCurrentControlScheme(device.displayName); // optional
                InputUser.PerformPairingWithDevice(device, leavingInput.user);
            }
            playerInputs[leavingIndex] = nextPlayer;
            readyStates[leavingIndex] = readyStates[index];
            playerInputs.Remove(index);
            readyStates.Remove(index);
            leavingIndex = index;
        }

        if (leavingInput.gameObject.activeSelf)
        {
            playerInputs.Remove(leavingInput.playerIndex);
            readyStates.Remove(leavingInput.playerIndex);
            Destroy(leavingInput.transform.root.gameObject);
        }

        RebuildPlayerInputs();
        MovePlayersToSpawnPoints();
        UpdateAllPlayerCameras();

        PlayerInputManager.instance.EnableJoining();
    }

    private void RebuildPlayerInputs()
    {
        var oldPlayers = playerInputs.Values.ToList();
        playerInputs.Clear();
        readyStates.Clear();

        for (int i = 0; i < oldPlayers.Count; i++)
        {
            playerInputs[i] = oldPlayers[i];
            readyStates[i] = false; // reset ready states if needed
        }
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode loadmode)
    {
        isMenu =
            scene.name == "SelectionScreen" ||
            scene.name == "AfterRace" ||
            scene.name == "MainMenu";

        playerInputs.Clear();

        foreach (var input in FindObjectsByType<PlayerInput>(FindObjectsSortMode.None))
        {
            playerInputs[input.playerIndex] = input;
            input.camera.enabled  = !isMenu;
        }

        switch (scene.name)
        {
            case "MainMenu":

                if (PlayerInputManager.instance)
                {
                    if (PlayerInputManager.instance.playerCount == 0)
                    {
                        FindAnyObjectByType<MainMenuUIController>().ShowJoinPopup();
                        PlayerInputManager.instance.EnableJoining();
                    }
                }
                break;
            case "SelectionScreen":
                if (PlayerInputManager.instance) PlayerInputManager.instance.EnableJoining();
                for (int i = 0; i < playerInputs.Count; i++)
                {
                    readyStates[i] = false;
                }
                break;
            case "AfterRace":
                GameObject.FindWithTag("Finish").GetComponent<TextMeshProUGUI>().text = Leaderboard.GetLeaderboardString();
                break;


        }
        if (isMenu)
        {
            RaceController rc = FindAnyObjectByType<RaceController>();
            UIList = FindFirstObjectByType<SelectionUIList>();
            allPlayersSpawned = false;
        }
        else if (!allPlayersSpawned)
        {
            allPlayersSpawned = true;
            if (PlayerInputManager.instance)
                PlayerInputManager.instance.DisableJoining();
        }

        for (int i = 0; i < playerInputs.Count; i++)
        {
            switch (playerInputs[i].playerIndex)
            {
                case 0:
                    playerInputs[i].SwitchCurrentActionMap(isMenu ? "UI" : "Player");
                    if (!isMenu) playerInputs[i].GetComponent<PlayerCamera>().MinimapPrep();
                    break;
                default:
                    if (SceneManager.GetActiveScene().name == "SelectionScreen") playerInputs[i].SwitchCurrentActionMap("UI");
                    else playerInputs[i].SwitchCurrentActionMap(isMenu ? "Disabled" : "Player");
                    break;
            }
        }
        Cursor.lockState = isMenu ? CursorLockMode.Confined : CursorLockMode.Locked;

        MovePlayersToSpawnPoints();
    }

    void MovePlayersToSpawnPoints()
    {
        var spawns = FindObjectsByType<SpawnPointVisualizer>(FindObjectsSortMode.None)
            .OrderBy(o => o.name)
            .ToArray();

        foreach (var kvp in playerInputs)
        {
            int index = kvp.Key;
            PlayerInput input = kvp.Value;

            if (!input || index >= spawns.Length)
                continue;

            var rb = input.GetComponent<Rigidbody>();
            if (rb)
                rb.linearVelocity = Vector3.zero;

            input.transform.SetPositionAndRotation(
                spawns[index].transform.position,
                spawns[index].transform.rotation
            );
            if (!isMenu)
            {
                RacerData rd = input.GetComponent<RacerData>();
                rd.SetName("Player " + (input.playerIndex + 1));
                rd.OnRacetrackScene();
            }
        }
    }

private void UpdateAllPlayerCameras()
    {
        int currentTotal = playerInputs.Count;
        foreach (var player in playerInputs)
        {
            var splitCam = player.Value.GetComponentInChildren<SplitScreenCamera>();
            if (splitCam != null)
            {
                splitCam.SetupCamera(currentTotal);
            }
        }
    }

    public void SetReady(PlayerInput input)
    {
        if (SceneManager.GetActiveScene().name != "SelectionScreen") return;
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


        UIList.OpenTrackSelection();
    }
    public void SetUnready(PlayerInput input)
    {
        if (SceneManager.GetActiveScene().name != "SelectionScreen") return;
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

    public void UnreadyAll()
    {
        for (int i = 0; i < playerInputs.Count; i++)
        {
            SetUnready(playerInputs[i]);
        }
    }
}
