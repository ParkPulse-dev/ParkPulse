using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class parking_script : MonoBehaviour
{

    public Rect getRectInParentSpace(RectTransform rect_trans)
    {
        Rect result = rect_trans.rect;
        Vector2 vector = rect_trans.offsetMin + Vector2.Scale(rect_trans.pivot, result.size);
        if ((bool)rect_trans.parent)
        {
            RectTransform component = rect_trans.parent.GetComponent<RectTransform>();
            if ((bool)component)
            {
                vector += Vector2.Scale(rect_trans.anchorMin, component.rect.size);
            }
        }

        result.x += vector.x;
        result.y += vector.y;
        return result;
    }

    // Start is called before the first frame update
    public GameObject carR, carL;
    RectTransform my_rect_trans;
    public Rect my_rect;
    void Start()
    {
        carR = GameObject.Find("CarR");
        carL = GameObject.Find("CarL");

        my_rect_trans = (RectTransform)this.transform.GetComponent<RectTransform>();
        my_rect = getRectInParentSpace(my_rect_trans);
        // Rect carR_rect = ((RectTransform)carR.transform.GetComponent<RectTransform>()).rect;
        // Debug.Log(carR_rect);
    }



    // Check if the object is colliding with any parking spot
    bool IsCollidingWithParkingSpot(GameObject car)
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(car.transform.position, car.GetComponent<Collider2D>().bounds.size, 0f);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("ParkingSpot"))
                return true;
        }
        return false;
    }



    void Update()
    {
        if (this.name == "ParkingR")
        {
            Rect carR_rect = getRectInParentSpace((RectTransform)carR.transform.GetComponent<RectTransform>());
            if (carR_rect.Overlaps(my_rect) && !IsCollidingWithParkingSpot(carR))
            {
                Debug.Log("CarR Win");//: parkingR:" + my_rect + " carR:" + carR_rect);
                                      //Application.Quit();
                Time.timeScale = 0f;
            }

        }
        else
        {
            Rect carL_rect = getRectInParentSpace((RectTransform)carL.transform.GetComponent<RectTransform>());
            if (carL_rect.Overlaps(my_rect))
            {
                Debug.Log("CarL Win");
                //Application.Quit();
                Time.timeScale = 0f;
            }
        }
    }
}
