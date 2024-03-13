using System.Collections;
using UnityEngine;

public class GameManagement : MonoBehaviour
{

    private bool isExplainedAccel = false;

    private GameObject objectToFollow;

    public GameObject playerPrefab;

    private CarController carController;

    public string popUpAccelExplain;

    void Start()
    {
        objectToFollow = GameObject.FindWithTag("Car");
        carController = objectToFollow.GetComponent<CarController>();
        StartCoroutine(CheckForObject());
    }

    IEnumerator CheckForObject()
    {
        bool flag = true;
        while (flag)
        {
            yield return new WaitForSeconds(0.2f); // Adjust the interval as needed
            // Check if the player pressed either left or right arrow keys
            bool isTurning = Input.GetKey(KeyCode.LeftArrow) ^ Input.GetKey(KeyCode.RightArrow);

            // Check if the player is not pressing up or down arrow keys
            bool isNotAccelerating = !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow);

            // Check if the conditions are met
            if (!isExplainedAccel && isTurning && isNotAccelerating && -0.1 < Mathf.Abs(carController.Acceleration) && Mathf.Abs(carController.Acceleration) < 0.1)
            {
                PopupSystem pop = gameObject.GetComponent<PopupSystem>();
                pop.PopUp(popUpAccelExplain);
                isExplainedAccel = true;
            }
            if (objectToFollow == null)
            {
                StartCoroutine(InstPlayer());
                ThreeSecTimer timer = ThreeSecTimer.GetInstance();
                timer.Begin(3);
                flag = false;
            }
        }
    }

    IEnumerator InstPlayer()
    {
        yield return new WaitForSeconds(1f);
        Debug.LogWarning("Should instatiate...");
        Instantiate(playerPrefab, gameObject.transform.position, gameObject.transform.rotation);
        Start();
    }
}

