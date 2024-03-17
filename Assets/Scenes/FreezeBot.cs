
using UnityEngine;

public class FreezeBot : MonoBehaviour
{
    GameObject CarBot;

    // Start is called before the first frame update
    void Start()
    {
        CarBot = GameObject.Find("BotPlayer");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject car = collision.gameObject;
        if (car.CompareTag("Car"))
        {
            BotController carController = CarBot.GetComponent<BotController>();
            if (carController != null)
            {
                carController.IsFrozen = true;
            }
            
        }

        // Call DisplayExplanation method of the GameManagement script with index 3
        GameManagement gameManager = FindObjectOfType<GameManagement>();
        if (gameManager != null)
        {
            gameManager.DisplayExplanation(3);
        }

        Destroy(gameObject);

    }
}
