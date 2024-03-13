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
        Destroy(gameObject);
    }
}
