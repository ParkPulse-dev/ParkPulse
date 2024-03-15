
using UnityEngine;

public class FreezeEnemy : MonoBehaviour
{
    GameObject car2;
    public Sprite Sprite;
    public Sprite AudiSprite;
    public Sprite SnowSprite;
    public SpriteRenderer SpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        car2 = GameObject.Find("Player2");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject car = collision.gameObject;
        if (car.CompareTag("Car"))
        {
            Car3controller carController = car2.GetComponent<Car3controller>();
            if (carController != null)
            {

                car2.GetComponent<SpriteRenderer>().sprite = SnowSprite;
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
