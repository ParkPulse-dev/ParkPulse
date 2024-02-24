using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform[] targets; // Serialize the array so it appears in the Inspector
    public float moveSpeed = 5f; // Adjust the speed of camera movement
    private int currentTargetIndex = 0;
    private Vector3 initialPosition;
    private CinemachineVirtualCamera virtualCamera;

    // Event to signal when camera has finished moving
    public event System.Action OnCameraFinishedMoving;

    private bool tourCompleted = false; // Flag to track whether the camera has completed its tour


    void Start()
    {
        initialPosition = transform.position; // Store the initial position of the camera
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        virtualCamera.Follow = null;
    }

    void Update()
    {
        if (!tourCompleted)
        {
            // Check if there are any targets to move towards
            if (currentTargetIndex < targets.Length)
            {
                // Calculate the direction towards the current target
                Vector3 direction = targets[currentTargetIndex].position - transform.position;
                direction.Normalize();

                // Move the camera towards the current target
                transform.Translate(moveSpeed * Time.deltaTime * direction);

                // If the camera is close enough to the target, move to the next target
                if (Vector3.Distance(transform.position, targets[currentTargetIndex].position) < 0.1f)
                {
                    currentTargetIndex++;

                    // Check if all targets have been reached
                    if (currentTargetIndex >= targets.Length)
                    {
                        // Invoke the event when all targets have been reached
                        OnCameraFinishedMoving?.Invoke();
                    }
                }
            }
            else
            {
                // If all targets have been visited, return to the initial position
                transform.position = Vector3.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);

                // Check if the camera has reached the initial position
                if (Vector3.Distance(transform.position, initialPosition) < 0.1f)
                {
                    // Re-enable follow on the virtual camera
                    virtualCamera.Follow = GameObject.FindGameObjectWithTag("Car").transform;
                    tourCompleted = true; // Set the flag to indicate tour completion
                }
                else
                {
                    // Disable follow on the virtual camera until the camera returns to the initial position
                    virtualCamera.Follow = null;
                }
            }
        }
    }

    public void FinishTourAndReturnToPlayer()
    {
        // Disable follow on the virtual camera
        virtualCamera.Follow = null;
        // Set tourCompleted to true to stop camera movement
        tourCompleted = true;
        // Transition the camera smoothly to follow the player
        StartCoroutine(TransitionToPlayer());
    }

    private IEnumerator TransitionToPlayer()
    {
        Vector3 targetPosition = GameObject.FindGameObjectWithTag("Car").transform.position;
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        // Set the camera to follow the player
        virtualCamera.Follow = GameObject.FindGameObjectWithTag("Car").transform;
    }
}
