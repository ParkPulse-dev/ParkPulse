using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class ParkingSpot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private TextMeshProUGUI correctionText;
    [SerializeField] private float winFontSize;
    public string text;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Car") || other.CompareTag("Car2") || other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            if (!IsCollidingWithParkingSpot(other.gameObject))
            {
                string carName;
                winText.alignment = TextAlignmentOptions.Center;
                winText.fontSize = winFontSize;
                correctionText.text = "";
                if (SceneManager.GetActiveScene().buildIndex == 0)
                {
                    carName = other.gameObject.name;
                    winText.text = carName + " Wins!"; // Display the win message
                    PopupSystem pop = gameObject.GetComponent<PopupSystem>();
                    pop.PopUp(text);
                }
                else
                {
                    carName = PhotonNetwork.NickName;
                    winText.text = carName + " Wins!"; // Display the win message

                    // Check ownership using PhotonView
                    PhotonView photonView = other.GetComponent<PhotonView>();
                    if (photonView != null && photonView.IsMine)
                    {
                        RoomManager.instance.UpdatePlayerScore(photonView.Owner.ActorNumber, 50);
                        RoomManager.instance.LoadNextScene();
                    }
                }
            }
            else
            {
                // Check ownership using PhotonView
                PhotonView photonView = other.GetComponent<PhotonView>();
                if (photonView != null && !photonView.IsMine)
                    return; // Only execute code for the local player's car
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
