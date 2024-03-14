using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Cinemachine;
using UnityEngine.SceneManagement;


public class PlayerControllerManager : MonoBehaviour
{
    public CameraFollow playerCameraPrefab;
    public PhotonView view;
    private GameObject player;

    void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (view.IsMine)
        {
            CreateController();
            RoomManager.instance.SetPlayerName(view.Owner.ActorNumber,view.Owner.NickName);
        }
    }

    void CreateController()
    {
        // Find the index of the local player in the player list
        int localPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;

        // Instantiate the appropriate player prefab based on the index of the local player
        string prefabName = (localPlayerIndex == 0) ? "OnlinePlayer1" : "OnlinePlayer2";
        
        // Select the spawn spot based on the player index
        Vector3 spawnPosition = (localPlayerIndex == 0) ? RoomManager.instance.GetSpot1() : RoomManager.instance.GetSpot2();

        player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", prefabName), spawnPosition, Quaternion.identity);

        // Find the virtual camera GameObject in the scene
        GameObject virtualCameraObject = GameObject.Find("Virtual Camera");
        if (virtualCameraObject != null)
        {
            // Get the CameraFollow component attached to the virtual camera
            CameraFollow cameraFollow = virtualCameraObject.GetComponent<CameraFollow>();
            if (cameraFollow != null)
            {
                // Set the objectToFollow variable to the player GameObject
                cameraFollow.objectToFollow = player;
                cameraFollow.startAgain();
            }
            else
            {
                Debug.LogError("CameraFollow component not found on the Virtual Camera GameObject.");
            }
        }
        else
        {
            Debug.LogError("Virtual Camera GameObject not found in the scene.");
        }
    }

    public void PlayerWins()
    {
        if (view.IsMine)
        {
            // Call the method to load the next scene
            view.RPC("LoadNextScene", RpcTarget.All);
        }
    }

    [PunRPC]
    private void LoadNextScene()
    {
        // Load the next scene
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        PhotonNetwork.LoadLevel(nextSceneIndex);
    }
}




