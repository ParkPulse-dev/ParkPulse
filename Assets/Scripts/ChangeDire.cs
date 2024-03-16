using UnityEngine;

public class ChangeDire : MonoBehaviour
{
    private GameObject player1;
    private GameObject player2;

    // Start is called before the first frame update
    void Start()
    {
        // Find player objects at the start to avoid frequent calls to GameObject.FindGameObjectWithTag
        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if player objects are null or inactive, and find them again if necessary
        if (player1 == null || !player1.activeSelf)
        {
            player1 = GameObject.FindGameObjectWithTag("Player1");
        }
        if (player2 == null || !player2.activeSelf)
        {
            player2 = GameObject.FindGameObjectWithTag("Player2");
        }

        GameObject collidedCar = collision.gameObject;
        if (collidedCar.CompareTag("Player1") && player2 != null)
        {
            CarController carController = player2.GetComponent<CarController>();
            if (carController != null)
            {
                carController.isChangeDire = true;
            }
        }
        else if (collidedCar.CompareTag("Player2") && player1 != null)
        {
            CarController carController = player1.GetComponent<CarController>();
            if (carController != null)
            {
                carController.isChangeDire = true;
            }
        }

        // Call DisplayExplanation method of the GameManagement script with index 3
        GameManagement gameManager = FindObjectOfType<GameManagement>();
        if (gameManager != null)
        {
            gameManager.DisplayExplanation(2);
        }

        Destroy(gameObject);
    }
}
