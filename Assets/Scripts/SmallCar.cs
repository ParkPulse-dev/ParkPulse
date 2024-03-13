using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallCar : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject car = collision.gameObject;
        if (car.CompareTag("Car"))
        {
            car.transform.localScale = Vector3.one * 0.5f;
        }

        // Call DisplayExplanation method of the GameManagement script with index 3
        GameManagement gameManager = FindObjectOfType<GameManagement>();
        if (gameManager != null)
        {
            gameManager.DisplayExplanation(1);
        }

        Destroy(gameObject);
    }
}
