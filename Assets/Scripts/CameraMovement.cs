using UnityEngine;
using Cinemachine;
using System.Collections;

public class FollowObjectController : MonoBehaviour
{
    public GameObject objectToFollow;
    private CinemachineVirtualCamera virtualCamera;

    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        if (virtualCamera != null)
        {
            // Initially set the object to follow
            virtualCamera.Follow = objectToFollow.transform; // Assign the transform component
        }
        else
        {
            Debug.LogError("Cinemachine Virtual Camera component not found!");
        }

        // Start the coroutine to check for the object
        StartCoroutine(CheckForObject());
    }

    IEnumerator CheckForObject()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f); // Adjust the interval as needed

            if (objectToFollow == null)
            {
                FindObjectToFollow();
            }
        }
    }

    void FindObjectToFollow()
    {
        GameObject newObject = GameObject.FindWithTag("Car"); // Example: Finding by tag
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
