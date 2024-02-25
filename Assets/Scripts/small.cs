using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class small : MonoBehaviour
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
        GameObject car = collision.gameObject;
        if (car.CompareTag("Car"))
        {
            car.transform.localScale = Vector3.one * 0.5f;

        }
        Destroy(this.gameObject);
    }
}
