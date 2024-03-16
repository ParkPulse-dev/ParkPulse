using UnityEngine;
using Cinemachine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FollowObjectController : MonoBehaviour
{
    public GameObject objectToFollow;
    private CinemachineVirtualCamera virtualCamera;

    private string playerTag;

    void Start()
    {

        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        if (virtualCamera != null)
        {
            // Initially set the object to follow
            virtualCamera.Follow = objectToFollow.transform;
        }
        else
        {
            Debug.LogError("Cinemachine Virtual Camera component not found!");
        }

        // Start the coroutine to check for the object
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            StartCoroutine(CheckForObject());
        }
        else
        {
            if (objectToFollow.CompareTag("Player1"))
                playerTag = "Player1";
            else playerTag = "Player2";
            StartCoroutine(CheckForNetworkObject(playerTag));
        }

    }

    IEnumerator CheckForObject()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            if (objectToFollow == null)
            {
                FindObjectToFollow();
            }
        }
    }

    void FindObjectToFollow()
    {
        GameObject newObject = GameObject.FindWithTag("Car");
        if (newObject != null)
        {
            objectToFollow = newObject;
            virtualCamera.Follow = objectToFollow.transform; // Assign the transform component
        }
        else
        {
            Debug.LogWarning("Player object not found!");
        }
    }

    IEnumerator CheckForNetworkObject(string tag)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            if (objectToFollow == null)
            {
                FindObjectToFollow(tag);
            }
        }
    }

    void FindObjectToFollow(string tag)
    {
        GameObject newObject = GameObject.FindWithTag(tag);
        if (newObject != null)
        {
            objectToFollow = newObject;
            virtualCamera.Follow = objectToFollow.transform; // Assign the transform component
        }
        else
        {
            Debug.LogWarning("Player object not found!");
        }
    }
}
