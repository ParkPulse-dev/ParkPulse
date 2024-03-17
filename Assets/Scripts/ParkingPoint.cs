using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.Collections;
using Photon.Chat;
public class ParkingSpot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private TextMeshProUGUI correctionText;
    public string text;

    private bool player1parked = false;
    private bool player2parked = false;

    private bool win = false;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (win == true || (other.CompareTag("Player1") && player1parked) || (other.CompareTag("Player2") && player2parked)) return;
        if (other.CompareTag("Car") || other.CompareTag("Car2") || other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            if (!IsCollidingWithParkingSpot(other.gameObject))
            {
                string carName;
                winText.alignment = TextAlignmentOptions.Center;
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
                    PhotonView photonView = other.GetComponent<PhotonView>();
                    CarController carController = other.gameObject.GetComponent<CarController>();
                    carController.parkQ++;
                    if (carController.parkQ < 2)
                    {
                        Debug.Log("ParkQ: "+ carController.parkQ);
                        if (other.CompareTag("Player1")) player1parked = true;
                        else player2parked = true;

                        if (photonView.IsMine)
                        {
                            StartCoroutine(ParkSuccess());
                        }
                        return;
                    }
                    carName = PhotonNetwork.NickName;
                    winText.text = carName + " Wins!"; // Display the win message

                    // Check ownership using PhotonView

                    if (photonView != null && photonView.IsMine)
                    {
                        RoomManager.instance.UpdatePlayerScore(photonView.Owner.ActorNumber, 50);
                        RoomManager.instance.LoadNextScene();
                        win = true;
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

    IEnumerator ParkSuccess()
    {
        winText.text = "Well done! hurry up for the second parking spot!";
        yield return new WaitForSeconds(3f);
        winText.text = "";
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
