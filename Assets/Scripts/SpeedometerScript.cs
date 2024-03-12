using UnityEngine;
using TMPro;

public class SpeedometerScript : MonoBehaviour
{
    [SerializeField] private float minAngle = 206f;
    [SerializeField] private float maxAngle = 9f;

    [SerializeField] private GameObject speedometerUI;
    [SerializeField] private GameObject needle;
    [SerializeField] private TextMeshProUGUI maxSpeedText; // Reference to the TextMeshPro object

    private float maxSpeed;
    private float speedChange;
    private CarController carController;

    private void Start()
    {
        carController = CarController.GetInstance();

        if (carController == null)
        {
            Debug.LogError("CarController reference is not set in Speedometer script.");
            return;
        }

        speedChange = carController.speedChange;
        maxSpeed = carController.MaxSpeed;

        // Initially deactivate the TextMeshPro object
        maxSpeedText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (carController != null)
        {
            // Calculate the current speed percentage between min and max speed
            float speedPercentage = Mathf.Clamp01((Mathf.Abs(carController.CurrentAcceleration) - speedChange) / (maxSpeed - speedChange));

            // Calculate the angle for the needle based on the speed percentage
            float targetAngle = Mathf.Lerp(minAngle, maxAngle, speedPercentage);

            // Rotate the needle to the target angle
            needle.transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);

            // Check if the car is at max speed
            if (Mathf.Abs(carController.CurrentAcceleration) >= maxSpeed)
            {
                // Activate the TextMeshPro object
                maxSpeedText.gameObject.SetActive(true);
            }
            else
            {
                // Deactivate the TextMeshPro object
                maxSpeedText.gameObject.SetActive(false);
            }
        }
    }
}
