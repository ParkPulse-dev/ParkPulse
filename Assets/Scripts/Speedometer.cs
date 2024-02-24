using UnityEngine;

public class Speedometer : MonoBehaviour
{
    [SerializeField] private float minAngle = 206f;
    [SerializeField] private float maxAngle = 9f;

    private float maxSpeed;
    private float speedChange;

    private void Start()
    {
        CarController carController = CarController.GetInstance();

        if (carController == null)
        {
            Debug.LogError("CarController reference is not set in Speedometer script.");
            return;
        }

        speedChange = carController.speedChange;
        maxSpeed = carController.MaxSpeed;
    }

    private void Update()
    {
        CarController carController = CarController.GetInstance();

        if (carController != null)
        {
            // Calculate the current speed percentage between min and max speed
            float speedPercentage = Mathf.Clamp01((Mathf.Abs(carController.CurrentAcceleration) - speedChange) / (maxSpeed - speedChange));

            // Calculate the angle for the needle based on the speed percentage
            float targetAngle = Mathf.Lerp(minAngle, maxAngle, speedPercentage);

            // Rotate the needle to the target angle
            transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);
        }
    }
}
