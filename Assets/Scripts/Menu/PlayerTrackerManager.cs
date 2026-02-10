using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
public class PlayerTrackerManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    public string scene = "Level1";
    private Dictionary<int, PlayerInput> playerInputs = new();
    private bool allPlayersSpawned = false;
    private bool isMenu = true;

    private Dictionary<int, bool> readyStates = new();
    private SelectionUIList UIList;
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

        if (SceneManager.GetActiveScene().name == "MainMenu") PlayerInputManager.instance.DisableJoining();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadmode)
    {
        playerInputs.Clear();

        foreach (var input in FindObjectsByType<PlayerInput>(FindObjectsSortMode.None))
        {
            playerInputs[input.playerIndex] = input;
        }

        isMenu =
            scene.name == "SelectionScreen" ||
            scene.name == "AfterRace" ||
            scene.name == "MainMenu";

        if (scene.name == "MainMenu")
        {
            Debug.Log(PlayerInputManager.instance);
            if (PlayerInputManager.instance)
            {
                if(PlayerInputManager.instance.playerCount == 0)
                {
                    PlayerInputManager.instance.EnableJoining();
                }                
            }
        }
        else if (scene.name == "SelectionScreen")
        {
            if (PlayerInputManager.instance) PlayerInputManager.instance.EnableJoining();
            for (int i = 0; i < playerInputs.Count; i++)
            {
                readyStates[i] = false;
            }
            MovePlayersToSpawnPoints();
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (isMenu)
        {
            RaceController rc = FindAnyObjectByType<RaceController>();
            if (rc != null) StartCoroutine(DestroyNextFrame(rc.gameObject));
        }

        if (!isMenu && !allPlayersSpawned)
        {
            allPlayersSpawned = true;
            if (PlayerInputManager.instance)
                PlayerInputManager.instance.DisableJoining();

            MovePlayersToSpawnPoints();
        }
        else if (isMenu)
        {
            UIList = FindFirstObjectByType<SelectionUIList>();
            allPlayersSpawned = false;
        }
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

            if (index == 0)
                input.GetComponent<PlayerCamera>()?.MinimapPrep();

            input.SwitchCurrentActionMap(isMenu ? "UI" : "Player");
            if (!isMenu) input.GetComponent<RacerData>().OnRacetrackScene();
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

    private System.Collections.IEnumerator DestroyNextFrame(GameObject raceController)
    {
        yield return null;
        Destroy(raceController);
    }

    private void Update()
    {
        Debug.Log($"Players:{PlayerInputManager.instance.playerCount} CanJoin:{PlayerInputManager.instance.joiningEnabled}" );
    }
}
