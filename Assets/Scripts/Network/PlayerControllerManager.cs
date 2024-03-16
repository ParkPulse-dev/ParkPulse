using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerControllerManager : MonoBehaviour
{
    public CameraFollow playerCameraPrefab;
    public PhotonView view;
    private GameObject player;
    private Vector3 spawnPosition;

    void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (view.IsMine)
        {
            CreateController();
            RoomManager.instance.SetPlayerName(view.Owner.ActorNumber, view.Owner.NickName);
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                RoomManager.instance.UpdatePlayerScore(view.Owner.ActorNumber, 0);
            }
            else
            {
                int currScore = RoomManager.instance.GetPlayerScore(view.Owner.ActorNumber);
                RoomManager.instance.UpdatePlayerScore(view.Owner.ActorNumber, currScore);
            }
            StartCoroutine(CheckForObject());
        }
    }


    IEnumerator CheckForObject()
    {
        bool flag = true;
        while (flag)
        {
            yield return new WaitForSeconds(0.3f); // Adjust the interval as needed
            if (player == null)
            {
                StartCoroutine(InstPlayer());
                flag = false;
            }

        }
    }

    IEnumerator InstPlayer()
    {
        yield return new WaitForSeconds(1f);
        Start();
    }


    void CreateController()
    {
        // Find the index of the local player in the player list
        int localPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;

        // Instantiate the appropriate player prefab based on the index of the local player
        string prefabName = (localPlayerIndex == 0) ? "OnlinePlayer1" : "OnlinePlayer2";

        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            // Select the spawn spot based on the player index
            spawnPosition = (localPlayerIndex == 0) ? RoomManager.instance.GetSpot1() : RoomManager.instance.GetSpot2();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            // Select the spawn spot based on the player index
            spawnPosition = (localPlayerIndex == 0) ? RoomManager.instance.GetSpot3() : RoomManager.instance.GetSpot4();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            // Select the spawn spot based on the player index
            spawnPosition = (localPlayerIndex == 0) ? RoomManager.instance.GetSpot5() : RoomManager.instance.GetSpot6();
        }

        player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", prefabName), spawnPosition, Quaternion.identity);

        // Find the virtual camera GameObject in the scene
        CameraFollowPlayer();

        // Adjust the opacity of the car
        ChangeOpacity();

    }



    void CameraFollowPlayer()
    {
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

    void ChangeOpacity()
    {
        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
        Color color = sr.color;
        color.a = 1f;
        sr.color = color;
        sr.sortingOrder = 3;
    }

    public void PlayerWins()
    {
        RoomManager.instance.LoadNextScene();
    }

}




