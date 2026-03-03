using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class PlayerTrackerManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    public static PlayerTrackerManager instance;
    public bool isTimeTrial = false;
    public bool isTimeTrialWithGhost = false;
    public string pathToGhost;

    private Dictionary<int, PlayerInput> playerInputs = new();
    private Dictionary<int, bool> readyStates = new();
    private bool allPlayersSpawned = false;
    private bool isMenu = true;

    private SelectionScreenController UIList;

    private void Awake()
    {
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
        CoroutineRunner.Run(SwapMap(input, "UI"));

        MovePlayersToSpawnPoints();
        UpdateAllPlayerCameras();

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            FindAnyObjectByType<MainMenuUIController>()?.ShowJoinPopup(false);
            PlayerInputManager.instance.DisableJoining();
        }
        else if (SceneManager.GetActiveScene().name == "SelectionScreen")
        {
            CoroutineRunner.Run(SelectObject(rd.index, FindAnyObjectByType<SelectionScreenController>().GetStartButton(rd.index)));
        }
    }

    public void HandlePlayerLeft(PlayerInput leavingInput)
    {
        if (!leavingInput)
            return;

        RacerData leavingData = leavingInput.GetComponent<RacerData>();
        UISelection.RemovePlayer(leavingData.GetComponent<UISelection>());
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
        EventSystem es = FindAnyObjectByType<EventSystem>();
        if (es) Destroy(es.gameObject);

        isMenu = scene.name == "MainMenu"
              || scene.name == "SelectionScreen"
              || scene.name == "AfterRace"
              || scene.name == "TimeTrialMenu"
              || scene.name == "TrackSelect"
              || scene.name == "TrackSelectTimeTrial";

        switch (scene.name)
        {
            case "SelectionScreen":
                isTimeTrial = false;
                foreach (var input in FindObjectsByType<PlayerInput>(FindObjectsSortMode.None))
                {
                    RacerData rd = input.GetComponent<RacerData>();
                    playerInputs[rd.index] = input;
                    UISelection.playerSelections[rd.index].SwapSelection(FindAnyObjectByType<SelectionScreenController>().GetStartButton(rd.index));
                }
                PlayerInputManager.instance.EnableJoining();
                readyStates.Clear();
                foreach (int index in playerInputs.Keys)
                    readyStates[index] = false;

                UIList = FindFirstObjectByType<SelectionScreenController>();
                break;
            case "TimeTrialMenu":
                isTimeTrial = true;
                for (int i = playerInputs.Count - 1; i > 0; i--)
                {
                    HandlePlayerLeft(playerInputs[i]);
                }
                PlayerInputManager.instance.DisableJoining();
                UIList = FindFirstObjectByType<SelectionScreenController>();
                UISelection.playerSelections[0].SwapSelection(FindAnyObjectByType<SelectionScreenController>().GetStartButton(0));
                break;
            case "MainMenu":
                if (playerInputs.Count < 1)
                {
                    PlayerInputManager.instance.EnableJoining();
                }
                break;
            case "AfterRace":
                GameObject.FindWithTag("Finish").GetComponent<TextMeshProUGUI>().text = Leaderboard.GetLeaderboardString();
                CoroutineRunner.Run(SelectObject(0, FindAnyObjectByType<SceneController>().GetComponent<UIButton>()));
                break;
            case "TrackSelect":
            case "TrackSelectTimeTrial":
                isTimeTrialWithGhost = false;
                CoroutineRunner.Run(SelectObject(0, FindFirstObjectByType<SelectionScreenController>().transform.GetChild(1).GetComponentInChildren<UIButton>()));
                break;
        }

        foreach (var kvp in playerInputs)
        {
            if (kvp.Value.camera) kvp.Value.camera.enabled = !isMenu;
            int index = kvp.Key;
            PlayerInput input = kvp.Value;

            if (index == 0)
            {
                CoroutineRunner.Run(SwapMap(input, isMenu ? "UI" : "Player"));
            }
            else
            {
                CoroutineRunner.Run(SwapMap(input,
                    (scene.name == "SelectionScreen")
                        ? "UI"
                        : (isMenu ? "Disabled" : "Player")
                ));
            }
        }

        Cursor.lockState = isMenu ? CursorLockMode.Confined : CursorLockMode.Locked;

        if (!isMenu)
        {
            if (!allPlayersSpawned)
            {
                allPlayersSpawned = true;
                PlayerInputManager.instance?.DisableJoining();
            }
            if (isTimeTrial)
            {
                Pickup[] pickups = FindObjectsByType<Pickup>(FindObjectsSortMode.None);
                for (int i = pickups.Length - 1; i >= 0; i--)
                {
                    if (pickups[i].powerUpType != PlayerPowerups.PowerUpType.gasolineTank)
                    {
                        Destroy(pickups[i].gameObject);
                    }
                }
            }
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
        if (SceneManager.GetActiveScene().name == "TimeTrialMenu")
        {
            SceneManager.LoadScene("TrackSelectTimeTrial");
        }
        if (SceneManager.GetActiveScene().name != "SelectionScreen") return;

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
        SceneManager.LoadScene("TrackSelect");
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
            SetUnready(input);
    }

    public int GetPlayerCount()
    {
        return playerInputs.Count;
    }

    private IEnumerator SelectObject(int index, UIButton button)
    {
        yield return null;
        if (UISelection.playerSelections.Count > index) UISelection.playerSelections[index].SwapSelection(button);
    }

    public IEnumerator SwapMap(PlayerInput input, string map)
    {
        if (!input)
            yield break;
        yield return null;

        if (!input.enabled)
            input.enabled = true;

        if (!input.actions.enabled)
        {
            input.actions.Enable();
            yield return null;
        }

        if (input.actions.FindActionMap(map, true) != null)
        {
            input.SwitchCurrentActionMap(map);
        }
    }

    public void SetGhostFile(string fullPath, bool isTimeTrialGhost)
    {
        pathToGhost = fullPath;
        isTimeTrialWithGhost = isTimeTrialGhost;
        Debug.Log($"Ghost set to: {fullPath} | TimeTrialGhost: {isTimeTrialGhost}");
    }
}
