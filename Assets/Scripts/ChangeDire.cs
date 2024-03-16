using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDire : MonoBehaviour
{
    private GameObject player1;
    private GameObject player2;


    // Start is called before the first frame update
    void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player1 == null || player2 == null)
        {
            player1 = GameObject.FindGameObjectWithTag("Player1");
            player2 = GameObject.FindGameObjectWithTag("Player2");
        }
        
        GameObject collidedCar = collision.gameObject;
        if (collidedCar.CompareTag("Player1"))
        {
            CarController carController = player2.GetComponent<CarController>();
            if (carController != null)
            {
                carController.isChangeDire = true;
            }
        }
        else if (collidedCar.CompareTag("Player2"))
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
