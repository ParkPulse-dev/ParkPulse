using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;

public class RoomManager : MonoBehaviourPunCallbacks
{

    public static RoomManager instance;
    public Vector3 spawnSpot1;
    public Vector3 spawnSpot2;

    // Dictionary to store player names, mapped by PhotonPlayer ID
    private Dictionary<int, string> playerNames = new Dictionary<int, string>();

    // Text fields for Player 1 and Player 2 names
    public TextMeshProUGUI player1NameText;
    public TextMeshProUGUI player2NameText;

    void Awake()
    {

        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;

    }

    // Method to set player name
    public void SetPlayerName(int playerId, string playerName)
    {
        playerNames[playerId] = playerName;
        // Update player name on the TextMeshProUGUI components if available
        UpdatePlayerNameOnScene();
        // Send updated player name to all other players
        photonView.RPC("SyncPlayerName", RpcTarget.OthersBuffered, playerId, playerName);
    }

    // RPC method to synchronize player names across the network
    [PunRPC]
    private void SyncPlayerName(int playerId, string playerName)
    {
        playerNames[playerId] = playerName;
        // Update player name directly on TextMeshProUGUI components
        UpdatePlayerNameOnScene();
    }

    // Update player name on TextMeshProUGUI components in the scene
    private void UpdatePlayerNameOnScene()
    {
        // Update player name on TextMeshProUGUI components
        if (player1NameText != null && playerNames.ContainsKey(1))
        {
            player1NameText.text = playerNames[1];
        }

        if (player2NameText != null && playerNames.ContainsKey(2))
        {
            player2NameText.text = playerNames[2];
        }
    }

    // Method to get player name by player ID
    public string GetPlayerName(int playerId)
    {
        if (playerNames.ContainsKey(playerId))
            return playerNames[playerId];
        else
            return "Unknown";
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex == 2)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerControllerManager"), Vector3.zero, Quaternion.identity);
        }
    }

    public Vector3 GetSpot1()
    {
        return spawnSpot1;
    }
    public Vector3 GetSpot2()
    {
        return spawnSpot2;
    }

}
