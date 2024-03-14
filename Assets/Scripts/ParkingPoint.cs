using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class ParkingSpot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private TextMeshProUGUI correctionText;
    [SerializeField] private int winFontSize = 4;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Car") || other.CompareTag("Car2"))
        {
            if (!IsCollidingWithParkingSpot(other.gameObject))
            {
                string carName = other.gameObject.name;
                winText.alignment = TextAlignmentOptions.Center;
                winText.fontSize = winFontSize;
                winText.text = carName + " Wins!"; // Display the win message
                correctionText.text = "";
                PopupSystem pop = gameObject.GetComponent<PopupSystem>();
                pop.PopUp("HELLO");
            }
            else
            {
                correctionText.text = "Car is not parking properly, try to balance and be more accurate...";
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        correctionText.text = "";
    }

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
}
