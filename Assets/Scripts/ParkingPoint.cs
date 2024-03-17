using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.Collections;
using Photon.Chat;
using System.Collections.Generic;
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
                        Debug.Log("ParkQ: " + carController.parkQ);
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
                        win = true;
                        RoomManager.instance.UpdatePlayerScore(photonView.Owner.ActorNumber, 50);

                        if (SceneManager.GetActiveScene().buildIndex == 5)
                        {
                            StartCoroutine(End());
                        }
                        else
                        {
                            StartCoroutine(LoadNextLevel());
                        }
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

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(3f);
        RoomManager.instance.LoadNextScene();
    }

    IEnumerator End()
    {
        yield return new WaitForSeconds(3f);
        winText.text = "";
        PopupSystem pop = GetComponent<PopupSystem>();

        string winTextEnd = "";
        List<int> winners = new List<int>();
        int highestScore = int.MinValue;

        // Iterate through player scores
        foreach (KeyValuePair<int, int> score in RoomManager.instance.playerScores)
        {
            // Append player ID and score to the win text
            winTextEnd += "Player " + score.Key + " Score: " + score.Value + "\n";

            // Check if current player's score is higher than the highest score
            if (score.Value > highestScore)
            {
                highestScore = score.Value;
                winners.Clear();
                winners.Add(score.Key);
            }
            else if (score.Value == highestScore)
            {
                // If the score is equal to the highest score, add the player to the list of winners
                winners.Add(score.Key);
            }
        }

        string winnerText;
        if (winners.Count == 1)
        {
            // If there's only one winner
            winnerText = "Player " + winners[0] + " Wins!";
        }
        else
        {
            // If there's a draw
            winnerText = "Draw!";
        }

        // Display the win text with scores and the winner(s)
        pop.PopUp("Final Scores:\n" + winTextEnd + "Winner: " + winnerText + "\nLet's go for another ride?");
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
