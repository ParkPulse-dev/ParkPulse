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
    private Dictionary<int, string> playerNames = new();

    // Dictionary to hold scores for each player
    private Dictionary<int, int> playerScores = new Dictionary<int, int>();

    // Text fields for Player 1 and Player 2 names
    public TextMeshProUGUI player1NameText;
    public TextMeshProUGUI player2NameText;

    // TMP Text for displaying scores
    public TMP_Text player1ScoreText;
    public TMP_Text player2ScoreText;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
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

        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerControllerManager"), Vector3.zero, Quaternion.identity);

    }

    public Vector3 GetSpot1()
    {
        return spawnSpot1;
    }
    public Vector3 GetSpot2()
    {
        return spawnSpot2;
    }

    // Method to update the score for a specific player
    public void UpdatePlayerScore(int playerId, int score)
    {
        if (playerScores.ContainsKey(playerId))
            playerScores[playerId] = score;
        else
            playerScores.Add(playerId, score);

        // Update the score display
        UpdateScoreText();

        // Send the updated score to all other players
        photonView.RPC("SyncPlayerScore", RpcTarget.OthersBuffered, playerId, score);
    }

    // RPC method to synchronize player scores across the network
    [PunRPC]
    private void SyncPlayerScore(int playerId, int score)
    {
        if (playerScores.ContainsKey(playerId))
            playerScores[playerId] = score;
        else
            playerScores.Add(playerId, score);

        // Update the score display
        UpdateScoreText();
    }

    // Method to update the score text display
    private void UpdateScoreText()
    {
        if (player1ScoreText != null)
            player1ScoreText.text = "Score: " + GetPlayerScore(1);

        if (player2ScoreText != null)
            player2ScoreText.text = "Score: " + GetPlayerScore(2);
    }

    // Method to get the score for a specific player
    public int GetPlayerScore(int playerId)
    {
        if (playerScores.ContainsKey(playerId))
            return playerScores[playerId];
        else
            return 0; // Return 0 if score not found
    }


}
