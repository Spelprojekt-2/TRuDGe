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

    private Dictionary<int, PlayerInput> playerInputs = new();
    private Dictionary<int, bool> readyStates = new();
    private bool allPlayersSpawned = false;

    private MenuController UIList;

    private void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneController.instance.SceneChangeEvent -= OnSceneLoaded;
        SceneController.instance.SceneChangeEvent += OnSceneLoaded;

        OnSceneLoaded();
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

        if (SceneController.instance.currentSceneType == SceneController.SceneType.MainMenu)
        {
            FindAnyObjectByType<MenuController>().ShowJoinPopup(false);
            PlayerInputManager.instance.DisableJoining();
        }
        else if (SceneController.instance.currentSceneType == SceneController.SceneType.PlayerSelectRace)
        {
            CoroutineRunner.Run(SelectObject(rd.index, UIList.GetStartButton(rd.index)));
        }
    }

    public void HandlePlayerLeft(PlayerInput leavingInput)
    {
        if (!leavingInput)
            return;

        RacerData leavingData = leavingInput.GetComponent<RacerData>();
        leavingData.GetComponent<UISelection>().RemovePlayer();
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
            UISelection.SwapPlayers(oldIndex, oldIndex +1);
        }

        MovePlayersToSpawnPoints();
        UpdateAllPlayerCameras();

        PlayerInputManager.instance.EnableJoining();
    }

    private void OnSceneLoaded()
    {
        EventSystem es = FindAnyObjectByType<EventSystem>();
        if (es) Destroy(es.gameObject);
        
        UIList = FindFirstObjectByType<MenuController>();

        //Remove bad player inputs
        var invalidKeys = playerInputs
    .Where(kvp => kvp.Value == null)
    .Select(kvp => kvp.Key)
    .ToList();

        foreach (var key in invalidKeys)
        {
            playerInputs.Remove(key);
        }

        switch (SceneController.instance.currentSceneType)
        {
            case SceneController.SceneType.PlayerSelectRace:
                RacingInformation.instance.isTimeTrial = false;

                List<PlayerInput> playerInputList = playerInputs.Values.ToList();
                foreach (var input in playerInputList)
                {
                    RacerData rd = input.GetComponent<RacerData>();
                    if (rd.index < UISelection.playerSelections.Count)
                    {
                        UISelection.playerSelections[rd.index].SwapSelection(UIList.GetStartButton(rd.index));
                    }
                }
                PlayerInputManager.instance.EnableJoining();
                readyStates.Clear();
                foreach (int index in playerInputs.Keys)
                    readyStates[index] = false;
                break;
            case SceneController.SceneType.PlayerSelectTimeTrial:
                RacingInformation.instance.isTimeTrial = true;
                for (int i = playerInputs.Count - 1; i > 0; i--)
                {
                    HandlePlayerLeft(playerInputs[i]);
                }
                PlayerInputManager.instance.DisableJoining();
                UISelection.playerSelections[0].SwapSelection(FindAnyObjectByType<MenuController>().GetStartButton(0));
                break;
            case SceneController.SceneType.MainMenu:
                if (playerInputs.Count < 1)
                {
                    PlayerInputManager.instance.EnableJoining();
                }
                break;
            case SceneController.SceneType.PostRaceLeaderboard:
                GameObject.FindWithTag("Finish").GetComponent<TextMeshProUGUI>().text = Leaderboard.GetLeaderboardString();
                CoroutineRunner.Run(SelectObject(0, FindAnyObjectByType<SceneController>().GetComponent<UIButton>()));
                break;
            case SceneController.SceneType.TrackSelectRace:
            case SceneController.SceneType.TrackSelectTimeTrial:
                RacingInformation.instance.isTimeTrialWithGhost = false;
                CoroutineRunner.Run(SelectObject(0, UIList.transform.GetChild(1).GetComponentInChildren<UIButton>()));
                break;
        }


        if (SceneController.instance.IsMenu)
        {
            foreach (var kvp in playerInputs)
            {
                if (kvp.Value.camera) kvp.Value.camera.enabled = false;
                int index = kvp.Key;
                PlayerInput input = kvp.Value;

                if (index == 0)
                {
                    CoroutineRunner.Run(SwapMap(input, "UI"));
                }
                else
                {
                    if (SceneController.instance.currentSceneType == SceneController.SceneType.PlayerSelectRace)
                        CoroutineRunner.Run(SwapMap(input, "UI"));
                    else CoroutineRunner.Run(SwapMap(input, "Disabled"));
                }
            }
            if (!allPlayersSpawned)
            {
                allPlayersSpawned = true;
                PlayerInputManager.instance.DisableJoining();
            }

        }
        else
        {
            foreach (var kvp in playerInputs)
            {
                if (kvp.Value.camera) kvp.Value.camera.enabled = true;
                PlayerInput input = kvp.Value;

                CoroutineRunner.Run(SwapMap(input, "Player"));

            }
        }

        Cursor.lockState = SceneController.instance.IsMenu ? CursorLockMode.None : CursorLockMode.Locked;
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

            if (!SceneController.instance.IsMenu)
            {
                RacerData rd = input.GetComponent<RacerData>();
                rd.SetName($"Player {rd.index + 1}");
            }
        }
    }

    private void UpdateAllPlayerCameras()
    {
        int total = playerInputs.Count;
        foreach (var player in playerInputs.Values)
        {
            SplitScreenCamera cam = player.GetComponentInChildren<SplitScreenCamera>();
            if (cam != null) cam.SetupCamera(total);
        }
    }

    public void SetReady(PlayerInput input)
    {
        if (SceneController.instance.currentSceneType == SceneController.SceneType.PlayerSelectTimeTrial)
        {
            SceneManager.LoadScene("TrackSelectTimeTrial");
        }
        if (SceneController.instance.currentSceneType != SceneController.SceneType.PlayerSelectRace) return;

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
        if (SceneController.instance.currentSceneType != SceneController.SceneType.PlayerSelectRace)
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
}
