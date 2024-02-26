using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDire : MonoBehaviour
{
    GameObject car2;


    // Start is called before the first frame update
    void Start()
    {
        car2 = GameObject.Find("Player22");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        GameObject car = collision.gameObject;
        if (car.CompareTag("Car"))
        {
            Car3controller carController = car2.GetComponent<Car3controller>();
            if (carController != null)
            {
                carController.IsChangeDire = true;
            }
        }
        Destroy(gameObject);

    }
}
