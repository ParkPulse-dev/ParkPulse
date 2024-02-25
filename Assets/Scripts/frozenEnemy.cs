using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frozenEnemy : MonoBehaviour
{
    GameObject car1;
    GameObject car2;


    // Start is called before the first frame update
    void Start()
    {
        car1 = GameObject.Find("Player1"); 
        car2 = GameObject.Find("Player22");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
        GameObject car = collision.gameObject;
        if (car.CompareTag("Car"))
        {
           Car3controller carController = car2.GetComponent<Car3controller>();
            if (carController != null)
            {
                carController.IsFrozen = true;
            }
        }
        
    }
}
