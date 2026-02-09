using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using System.Collections;

public class PlayerTrackerManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    public static PlayerTrackerManager instance;
    public static Dictionary<int, PlayerInput> playerInputs { get; private set; }
    private Dictionary<int, bool> readyStates = new();
    public static bool isMenu { get; private set; }
    private SelectionUIList UIList;
    private bool allPlayersSpawned = false;

    private void Awake()
    {
        playerInputs = new();
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;

        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    public void HandlePlayerJoined(PlayerInput input)
    {
        if (!input || playerInputs.ContainsValue(input))
            return;

        int index = playerInputs.Count;

        RacerData rd = input.GetComponent<RacerData>();
        rd.index = index;

        playerInputs[index] = input;
        readyStates[index] = false;

        DontDestroyOnLoad(input.transform.root.gameObject);

        input.SwitchCurrentActionMap("UI");

        MovePlayersToSpawnPoints();
        UpdateAllPlayerCameras();

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            FindAnyObjectByType<MainMenuUIController>()?.ShowJoinPopup(false);
            PlayerInputManager.instance.DisableJoining();

        }
    }

    public void HandlePlayerLeft(PlayerInput leavingInput)
    {
        if (!leavingInput)
            return;

        RacerData leavingData = leavingInput.GetComponent<RacerData>();
        int leavingIndex = leavingData.index;

        Destroy(leavingInput.transform.root.gameObject);

        playerInputs.Remove(leavingIndex);
        SetUnready(leavingInput);
        readyStates.Remove(leavingIndex);

        // Shift players above down
        var toShift = playerInputs
            .Where(p => p.Key > leavingIndex)
            .OrderBy(p => p.Key)
            .ToList();

        foreach (var kvp in toShift)
        {
            int oldIndex = kvp.Key;
            PlayerInput input = kvp.Value;

            RacerData rd = input.GetComponent<RacerData>();
            rd.index--;

            playerInputs.Remove(oldIndex);
            playerInputs[oldIndex - 1] = input;

            if (readyStates.TryGetValue(oldIndex, out bool wasReady))
            {
                readyStates[oldIndex - 1] = wasReady;
                readyStates.Remove(oldIndex);
            }
            SetUnready(kvp.Value);
        }

        MovePlayersToSpawnPoints();
        UpdateAllPlayerCameras();

        PlayerInputManager.instance.EnableJoining();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        isMenu = scene.name == "MainMenu"
              || scene.name == "SelectionScreen"
              || scene.name == "AfterRace";


        foreach (var input in FindObjectsByType<PlayerInput>(FindObjectsSortMode.None))
        {
            RacerData rd = input.GetComponent<RacerData>();
            playerInputs[rd.index] = input;
            input.camera.enabled = !isMenu;
        }

        switch (scene.name)
        {
            case "SelectionScreen":
                {
                    if (playerInputs.Count < 4) PlayerInputManager.instance.EnableJoining();
                    readyStates.Clear();
                    foreach (int index in playerInputs.Keys)
                        readyStates[index] = false;

                    UIList = FindFirstObjectByType<SelectionUIList>();
                    break;
                }

            case "MainMenu":
                if (playerInputs.Count < 1)
                {
                    PlayerInputManager.instance.EnableJoining();
                }
                break;
            case "AfterRace":
                GameObject.FindWithTag("Finish").GetComponent<TextMeshProUGUI>().text = Leaderboard.GetLeaderboardString();
                break;
        }

        StartCoroutine(Coroutines.SwapMaps());

        Cursor.lockState = isMenu ? CursorLockMode.Confined : CursorLockMode.Locked;

        if (!isMenu && !allPlayersSpawned)
        {
            allPlayersSpawned = true;
            PlayerInputManager.instance?.DisableJoining();
        }

        MovePlayersToSpawnPoints();
    }

    private void MovePlayersToSpawnPoints()
    {
        var spawns = FindObjectsByType<SpawnPointVisualizer>(FindObjectsSortMode.None)
            .OrderBy(s => s.name)
            .ToArray();

        foreach (var kvp in playerInputs)
        {
            int index = kvp.Key;
            PlayerInput input = kvp.Value;

            if (!input || index >= spawns.Length)
                continue;

            var rb = input.GetComponent<Rigidbody>();
            if (rb) rb.linearVelocity = Vector3.zero;

            input.transform.SetPositionAndRotation(
                spawns[index].transform.position,
                spawns[index].transform.rotation
            );

            if (!isMenu)
            {
                RacerData rd = input.GetComponent<RacerData>();
                rd.SetName($"Player {rd.index + 1}");
                rd.OnRacetrackScene();
            }
        }
    }

    private void UpdateAllPlayerCameras()
    {
        int total = playerInputs.Count;
        foreach (var player in playerInputs.Values)
        {
            var cam = player.GetComponentInChildren<SplitScreenCamera>();
            cam?.SetupCamera(total);
        }
    }

    public void SetReady(PlayerInput input)
    {
        if (SceneManager.GetActiveScene().name != "SelectionScreen")
            return;

        int player = input.GetComponent<RacerData>().index;

        if (readyStates[player]) return;

        readyStates[player] = true;

        switch (player)
        {
            case 0: UIList.ReadyTextP1.gameObject.SetActive(true); break;
            case 1: UIList.ReadyTextP2.gameObject.SetActive(true); break;
            case 2: UIList.ReadyTextP3.gameObject.SetActive(true); break;
            case 3: UIList.ReadyTextP4.gameObject.SetActive(true); break;
        }

        foreach (bool ready in readyStates.Values)
        {
            if (!ready)
                return;
        }

        PlayerInputManager.instance.DisableJoining();
        UIList.OpenTrackSelection();

        foreach (var kvp in playerInputs)
        {
            int index = kvp.Key;
            Debug.Log(index);
            PlayerInput inputs = kvp.Value;
            inputs.SwitchCurrentActionMap(index == 0 ? "UI" : "Disabled");
        }
        var inputModule = EventSystem.current.currentInputModule as InputSystemUIInputModule;
        inputModule.actionsAsset = playerInputs[0].actions;
        inputModule.enabled = false;
        inputModule.enabled = true;
    }

    public void SetUnready(PlayerInput input)
    {
        if (SceneManager.GetActiveScene().name != "SelectionScreen")
            return;

        int player = input.GetComponent<RacerData>().index;
        readyStates[player] = false;

        switch (player)
        {
            case 0: UIList.ReadyTextP1.gameObject.SetActive(false); break;
            case 1: UIList.ReadyTextP2.gameObject.SetActive(false); break;
            case 2: UIList.ReadyTextP3.gameObject.SetActive(false); break;
            case 3: UIList.ReadyTextP4.gameObject.SetActive(false); break;
        }
    }

    public void UnreadyAll()
    {
        foreach (var input in playerInputs.Values)
        {
            SetUnready(input);
            input.SwitchCurrentActionMap("UI");
        }
    }

    public int GetPlayerCount()
    {
        return playerInputs.Count;
    }
}
public static class Coroutines
{ 
    public static IEnumerator SwapMaps()
    {
        yield return null;
        foreach (var kvp in PlayerTrackerManager.playerInputs)
        {
            int index = kvp.Key;
            PlayerInput input = kvp.Value;

            if (index == 0)
            {
                input.SwitchCurrentActionMap(PlayerTrackerManager.isMenu ? "UI" : "Player");
                if (!PlayerTrackerManager.isMenu) input.GetComponent<PlayerCamera>()?.MinimapPrep();
            }
            else
            {
                input.SwitchCurrentActionMap(
                    SceneManager.GetActiveScene().name == "SelectionScreen"
                        ? "UI"
                        : (PlayerTrackerManager.isMenu ? "Disabled" : "Player")
                );
            }
        }
    }

    public static IEnumerator SelectButton(GameObject gameObject)
    {
        yield return null;
        MainMenuUIController.SelectObject(gameObject);
    }
}
