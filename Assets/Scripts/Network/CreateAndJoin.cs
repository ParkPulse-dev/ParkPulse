using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public TMP_Text waitingText;

    public void CreateRoom()
    {
        Debug.Log("Attempting to join an existing room or create a new one...");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No random room available, creating a new one...");
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 2 }; // Set the maximum number of players
        PhotonNetwork.CreateRoom(null, roomOptions); // Passing null as room name will make Photon generate a unique name
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log("Current player count: " + PhotonNetwork.CurrentRoom.PlayerCount);

        if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
        {
            Debug.Log("Waiting for opponent...");
            waitingText.text = "Waiting for opponent...";
        }
        else if (PhotonNetwork.IsMasterClient)
        {
            // If the current client is the master and there are already two players in the room, load the scene
            LoadGamePlayScene();
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("A player has entered the room.");
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && PhotonNetwork.IsMasterClient)
        {
            LoadGamePlayScene();
        }
    }

    private void LoadGamePlayScene()
    {
        Debug.Log("Room has 2 players. Master client loading GamePlay scene...");
        PhotonNetwork.LoadLevel("Level1");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room: " + message);
        CreateRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning("Disconnected from Photon network: " + cause);
        // Handle disconnection, e.g., return to the main menu or show an error message.
    }

    public void SetPlayerName(string playerName)
    {
        PhotonNetwork.NickName = playerName;
    }
}
